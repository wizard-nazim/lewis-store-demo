namespace LewisStore.Dtos;

public record PaymentDto(decimal Amount, Guid? OrderId, Guid? CreditAgreementId, string? Method, string? Reference);
