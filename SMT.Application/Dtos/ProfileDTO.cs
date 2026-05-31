namespace SMT.Application.Dtos;

public record ProfileDTO(
    Guid UserId,
    DateTime IpRegistrationDateTime,
    string CompanyName
);