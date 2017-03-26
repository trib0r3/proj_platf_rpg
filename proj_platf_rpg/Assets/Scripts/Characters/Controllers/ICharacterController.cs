public interface ICharacterController
{
  string controllerType { get; }

  float moveDirection { get; }
  bool isJumpClicked { get; }
  bool isRunningKeyClicked { get; }

  void Control();
}
