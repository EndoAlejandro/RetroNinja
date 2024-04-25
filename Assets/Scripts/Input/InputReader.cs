using UnityEngine;

namespace SuperKatanaTiger.Input
{
    public class InputReader
    {
        private static InputReader Instance => _instance ??= new InputReader();
        private static InputReader _instance;
        private readonly NinjaInput _input;

        public static Vector3 Movement => Instance.Moving();
        public static Vector2 Aim => Instance.Aiming();
        public static bool Attack => Instance._input.Main.Attack.WasPerformedThisFrame();
        public static bool Parry => Instance._input.Main.Parry.WasPerformedThisFrame();
        public static bool Run => Instance._input.Main.Run.IsPressed();
        public static bool Pause => Instance._input.Main.Pause.WasPerformedThisFrame();

        private InputReader()
        {
            _input = new NinjaInput();
            _input.Main.Enable();
        }

        private Vector3 Moving()
        {
            var movement = _input.Main.Move.ReadValue<Vector2>();
            return new Vector3(movement.x, 0f, movement.y);
        }
        
        private Vector2 Aiming()
        {
            var mouseScreen = _input.Main.Aim.ReadValue<Vector2>();
            return new Vector2(mouseScreen.x / Screen.width, mouseScreen.y / Screen.height); //- Vector2.one / 2;
        }
    }
}