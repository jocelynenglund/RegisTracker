namespace RegisTrackerSystem.Domain;

public record RegistrationStatistics(int Total, Dictionary<int, int> BatchCount);
