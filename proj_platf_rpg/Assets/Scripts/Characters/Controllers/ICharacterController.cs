public interface ICharacterController
{
  string controllerType { get; }

  float moveDirection { get; }
  bool isJumpClicked { get; }
  bool isRunningKeyClicked { get; }
  bool isAttackClicked { get; }

  void Control();
}
