namespace PrecisionPressureController.Domain
{
    /// <summary>
    /// Состояние сессии
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// Сессия не создана
        /// </summary>
        None = 0,
        
        /// <summary>
        /// Сессия создана, но не запущена
        /// </summary>
        Created = 1,
        
        /// <summary>
        /// Сессия активна (запись данных идет)
        /// </summary>
        Active = 2,
        
        /// <summary>
        /// Сессия завершена (read-only режим)
        /// </summary>
        Completed = 3
    }
}


