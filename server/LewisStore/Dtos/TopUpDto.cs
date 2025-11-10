namespace LewisStore.Dtos;

public record TopUpDto(decimal Amount);
// This is used for specifying an amount to top up a user's balance. 
// This is primarily used by admin controllers to add balance to user accounts.
// Users can also top up their balance via the payment service which also uses this DTO. 
