using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Domain.Models
{
    public static class DomainErrorMessages
    {
        public static readonly string NameIsRequired = "Nome é obrigatório";
        public static readonly string DocumentIsNotValid = "Documento não é válido";
        public static readonly string EmailIsRequired = "E-mail é obrigatório";
        public static readonly string EmailIsNotValid = "E-mail não é válido";
        public static readonly string CommandIsNotValid = "Comando não é válido";
        public static readonly string QueryIsNotValid = "Consulta não é válida";
        public static readonly string CustomerNotFound = "Cliente não encontrado";
        public static readonly string CreatedAtIsRequired = "CreatedAt é obrigatório";
        public static readonly string UpdatedAtIsRequired = "UpdatedAt é obrigatório";
        public static readonly string RequestSuccess = "Requisição valida";
        public static readonly string BadRequest = "Requisição inválida";
        public static readonly string BarcodeError = "Código de barras é inválido ou não pode ser lido";
        public static readonly string DateIsInThePast = "A data não pode ser no passado";
        public static readonly string BoletoNotFound = "Boleto não encontrado";
        public static readonly string BoletoLimitExceeded = "Limite do boleto excedido";
        public static readonly string DailyLimitExceeded = "Limite diário excedido";
        public static readonly string InsufficientFundsForPayment = "Sem saldo para pagamento";
        public static readonly string PaymentNotFound = "Pagamento não encontrado";
        public static readonly string PaymentSucess = "Pagamento Efetuado Com sucesso";
        public static readonly string PaymentCancel = "Pagamento Cancelado";
    }
}
