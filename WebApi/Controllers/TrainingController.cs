using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Application.Services;
using Application.DTO;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainingController : Controller
{
    private readonly TrainingService _trainingService;

    List<string> _errorMessages = new List<string>();

    public TrainingController(TrainingService trainingService)
    {
        _trainingService = trainingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TrainingDTO>>> GetTrainings()
    {
        IEnumerable<TrainingDTO> trainingsDTO = await _trainingService.GetAll();
        return Ok(trainingsDTO);
    }
    //
    [HttpGet("{id}")]
    public async Task<ActionResult<TrainingDTO>> GetTrainingById(long id)
    {
        var trainingDTO = await _trainingService.GetById(id);
        if (trainingDTO is not null)
        {
            return Ok(trainingDTO);
        }
    
        return NotFound();
    }
    
    // [HttpPut("{id}")]
    // public async Task<IActionResult> Puttraining(long id, TrainingDTO trainingDTO)
    // {
    //     if (id != trainingDTO.Id)
    //     {
    //         return BadRequest();
    //     }
    //
    //     bool wasUpdated = await _trainingService.Update(id, trainingDTO, _errorMessages, true);
    //     if (!wasUpdated)
    //     {
    //         return BadRequest(_errorMessages);
    //     }
    //
    //     return Ok();
    // }
    //
    // [HttpPost]
    // public async Task<ActionResult<IEnumerable<TrainingDTO>>> PostTraining(TrainingDTO trainingDTO)
    // {
    //     TrainingDTO trainingResultDTO = await _trainingService.AddFromRest(trainingDTO, _errorMessages);
    //     if (trainingResultDTO is not null)
    //     {
    //         return Created("", trainingResultDTO);
    //     }
    //     else
    //     {
    //         return BadRequest(_errorMessages);
    //     }
    // }
}