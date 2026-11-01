namespace DemoApi.Domain;

/// <summary>Проект</summary>
/// <param name="Id">Идентификатор</param>
/// <param name="Name">Название, уникальное</param>
/// <param name="CreatedAt">Когда создан</param>
public sealed record Project(int Id, string Name, DateTimeOffset CreatedAt);

/// <summary>
/// Статистика задач по проекту.
/// </summary>
/// <param name="ProjectId">Идентификатор проекта.</param>
/// <param name="Total">Общее количество задач.</param>
/// <param name="Todo">Количество задач в статусе Todo.</param>
/// <param name="InProgress">Количество задач в статусе InProgress.</param>
/// <param name="Done">Количество выполненных задач.</param>
/// <param name="Overdue">Количество просроченных задач.</param>
public record ProjectStats(
    int ProjectId,
    int Total,
    int Todo,
    int InProgress,
    int Done,
    int Overdue
);