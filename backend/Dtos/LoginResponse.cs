namespace Backend.Dtos;

public record LocationDto(string LocationCode, string LocationName);

public record LoginResponse(string Token, string Email, List<LocationDto> Locations);
