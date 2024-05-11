namespace Application.DTO;

using Domain.Model;

public class TrainingDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly? EndDate { get; set; }

    public TrainingDTO()
    {
    }

    public TrainingDTO(long id, string name, DateOnly startDate, DateOnly? endDate)
    {
        Id = id;
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }

    public static TrainingDTO ToDTO(Training training)
    {
        TrainingDTO trainingDto = new TrainingDTO(training.Id, training.Name, training.StartDate, training.EndDate);
        return trainingDto;
    }
    
    public static IEnumerable<TrainingDTO> ToDTO(IEnumerable<Training> trainings)
    {
        List<TrainingDTO> trainingsDTO = new List<TrainingDTO>();
        foreach (Training training in trainings)
        {
            TrainingDTO trainingDTO = ToDTO(training);
            trainingsDTO.Add(trainingDTO);
        }
        return trainingsDTO;
    }

    public static Training ToDomain(TrainingDTO trainingDto)
    {
        Training training = new Training(trainingDto.Name, trainingDto.StartDate, trainingDto.EndDate);
        return training;
    }

    public static Training UpdateToDomain(Training training, TrainingDTO trainingDTO)
    {
        training.SetName(trainingDTO.Name);
        training.SetEndDate(trainingDTO.EndDate);
        return training;
    }
}