using MediatR;
using MotorcycleRental.Application.Commands;
using MotorcycleRental.Application.Queries;
using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Interfaces;


namespace MotorcycleRental.Application.Handlers
{
    
    public class GetAllPlansHandler : IRequestHandler<GetAllPlansQuery, List<Plan>>
    {
        private readonly IPlanRepository _repository;

        public GetAllPlansHandler(IPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Plan>> Handle(GetAllPlansQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllPlansAsync();
        }
    }

    public class GetPlanByIdHandler : IRequestHandler<GetPlanByIdQuery, Plan>
    {
        private readonly IPlanRepository _repository;

        public GetPlanByIdHandler(IPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<Plan> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetPlanByIdAsync(request.Id);
        }
    }

    public class CreatePlanHandler : IRequestHandler<CreatePlanCommand, string>
    {
        private readonly IPlanRepository _repository;

        public CreatePlanHandler(IPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> Handle(CreatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = new Plan
            {
                Days = request.Days,
                CostPerDay = request.CostPerDay,
                PenaltyPercentage = request.PenaltyPercentage
            };
            await _repository.AddPlanAsync(plan);
            return plan.Id;
        }
    }

    public class UpdatePlanHandler : IRequestHandler<UpdatePlanCommand>
    {
        private readonly IPlanRepository _repository;

        public UpdatePlanHandler(IPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(UpdatePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _repository.GetPlanByIdAsync(request.Id);
            if (plan == null) throw new KeyNotFoundException("Plan not found.");
            plan.Days = request.Days;
            plan.CostPerDay = request.CostPerDay;
            plan.PenaltyPercentage = request.PenaltyPercentage;
            await _repository.UpdatePlanAsync(plan);
            return Unit.Value;
        }
    }

    public class DeletePlanHandler : IRequestHandler<DeletePlanCommand>
    {
        private readonly IPlanRepository _repository;

        public DeletePlanHandler(IPlanRepository repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeletePlanCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeletePlanAsync(request.Id);
            return Unit.Value;
        }
    }


}