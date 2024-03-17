namespace ToolCart.Foundation;

/// <summary>
/// Default error codes.
/// </summary>
public enum ErrorCode
{
  /// <summary>
  /// No error.
  /// </summary>
  None = 0x000,

  /// <summary>
  /// Executions keep failing.
  /// </summary>
  ConsecutiveFailures = 0x065,

  /// <summary>
  /// Missing requirements. 
  /// (e.g. mandatory argument)
  /// </summary>
  MissingRequirements = 0x0C9,

  /// <summary>
  /// Execution failed due to 
  /// a critical exception.
  /// </summary>
  CriticalError = 0x12D
}
