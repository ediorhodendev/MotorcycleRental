using Amazon.S3;
using Amazon.S3.Transfer;
using MediatR;
using Microsoft.AspNetCore.Http;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;



namespace MotorcycleRental.Application.Handlers
{
    public class DeliveryHandlers :
       IRequestHandler<RegisterDeliveryCommand, string>,
       IRequestHandler<UpdateDeliveryCommand, string>,
       IRequestHandler<RentMotorcycleCommand, string>,
       IRequestHandler<GetDeliveryQuery, Delivery>,
       IRequestHandler<GetAllDeliverysQuery, List<Delivery>>
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IMotorcycleRepository _motorcycleRepository;
      

        public DeliveryHandlers(IDeliveryRepository deliveryRepository, IMotorcycleRepository motorcycleRepository)
        {
            _deliveryRepository = deliveryRepository;
            _motorcycleRepository = motorcycleRepository;
      
        }

        public async Task<string> Handle(RegisterDeliveryCommand request, CancellationToken cancellationToken)
        {
            // Validate CNPJ and License Number uniqueness before adding a new delivery person
            if (await _deliveryRepository.CheckCNPJExistsAsync(request.Cnpj) ||
                await _deliveryRepository.CheckLicenseNumberExistsAsync(request.LicenseNumber))
            {
                throw new InvalidOperationException("CNPJ or license number already exists and must be unique.");
            }

            var delivery = new Delivery
            {
                Name = request.Name,
                Cnpj = request.Cnpj,
                LicenseNumber = request.LicenseNumber,
                LicenseType = request.LicenseType,
                BirthDate = request.BirthDate
            };

            // Upload image to S3 bucket
            var imageUrl = await UploadImageToS3(request.LicenseImage);
            delivery.LicenseImagePath = imageUrl;

            await _deliveryRepository.AddDeliveryAsync(delivery);
            return delivery.Id;
        }


        public async Task<string> UploadImageToS3(IFormFile imageFile)
        {
           

           string _accessKey = "AKIAW3MD7SQQZSQD3VXX";
           string  _secretKey = "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";
           string  _bucketName = "motorcyclerentalapi";

            try
            {
                var credentials = new Amazon.Runtime.BasicAWSCredentials(_accessKey, _secretKey);
                var config = new AmazonS3Config
                {
                    RegionEndpoint = Amazon.RegionEndpoint.USEast2
                };
                var s3Client = new AmazonS3Client(credentials, config);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imageFile.FileName)}";

                using (var fileStream = imageFile.OpenReadStream())
                {
                    var fileTransferUtility = new TransferUtility(s3Client);
                    //await fileTransferUtility.UploadAsync(fileStream, _bucketName, fileName);
                }
                return "url-teste-arquivofoto";
                //return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
            }
            catch (AmazonS3Exception ex)
            {
               
                return null;
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }
    
    public async Task<List<Delivery>> Handle(GetAllDeliverysQuery request, CancellationToken cancellationToken)
        {
            return (List<Delivery>)await _deliveryRepository.GetAllDeliverysAsync();
        }
        public async Task<string> Handle(UpdateDeliveryCommand request, CancellationToken cancellationToken)
        {
            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(request.Id);
            if (delivery == null)
            {
                throw new InvalidOperationException("Delivery person not found with ID: " + request.Id);
            }

            delivery.Name = request.Name; 
            await _deliveryRepository.UpdateDeliveryAsync(delivery);
            return request.Id;
        }

        public async Task<Delivery> Handle(GetDeliveryQuery request, CancellationToken cancellationToken)
        {
            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(request.Id);
            return delivery;
        }

        public async Task<string> Handle(RentMotorcycleCommand request, CancellationToken cancellationToken)
        {
            var delivery = await _deliveryRepository.GetDeliveryByIdAsync(request.DeliveryPersonId);
            if (delivery == null || !delivery.LicenseType.Contains("A"))
            {
                throw new InvalidOperationException("Delivery person is not eligible for renting a motorcycle due to license restrictions.");
            }

            await _motorcycleRepository.RentMotorcycleAsync(request.MotorcycleId, request.DeliveryPersonId, request.StartDate, request.EndDate);
            return $"Motorcycle {request.MotorcycleId} rented to delivery person {request.DeliveryPersonId}";
        }
    }
}
