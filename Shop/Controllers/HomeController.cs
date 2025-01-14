using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Shop.Models;

namespace Shop.Controllers;

public enum Position {
    Developer,
    Manager
}

public interface IQuestionRepository {
    List<string> GetQuestions(Position position);
}

public class QuestionRepository : IQuestionRepository {
    private readonly Dictionary<Position, List<string>> _questions = new() {
        { Position.Developer, ["Что такое ООП?", "Что такое SOLID?"] },
        { Position.Manager, ["Как вы управляете командой?", "Какие методологии Agile знаете?"] }
    };

    public List<string> GetQuestions(Position position) {
        return _questions.TryGetValue(position, out var question) ? question : ["Вопросы не найдены."];
    }
}

public class InterviewController(IQuestionRepository repository) : Controller {
    public IActionResult Index() {
        return View(new InterviewViewModel());
    }

    [HttpPost]
    public IActionResult GetQuestions(Position position) {
        var questions = repository.GetQuestions(position);
        return View("Index", new InterviewViewModel { SelectedPosition = position, Questions = questions });
    }
}

public class InterviewViewModel {
    public Position? SelectedPosition { get; set; } = null;
    public List<string> Questions { get; set; } = [];
}