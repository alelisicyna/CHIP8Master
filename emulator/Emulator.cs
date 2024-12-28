/* using System;
using System.Numerics;
using Raylib_cs;

class Emulator {
    // Size of window
    static int Increase = 20;
    static int[] WindowSize = [64 * Increase, 32 * Increase];

    // CPU
    public static byte[] Memory = new byte[4096];
    public static int PC = 0;
    public static ushort[] Registers = new ushort[16];
    public static int I = 0;
    public static ushort[] Stack = new ushort[16];
    public static int SP = -1;
    public static int SoundTimer = 0;
    public static int DelayTimer  = 0;

    static Dictionary<KeyboardKey, int> KeySet = new Dictionary<KeyboardKey, int>()
    {
        [KeyboardKey.One] = 1,
        [KeyboardKey.Two] = 2,
        [KeyboardKey.Three] = 3,
        [KeyboardKey.Four] = 12,
        [KeyboardKey.Q] = 4,
        [KeyboardKey.W] = 5,
        [KeyboardKey.E] = 6,
        [KeyboardKey.R] = 13,
        [KeyboardKey.A] = 7,
        [KeyboardKey.S] = 8,
        [KeyboardKey.D] = 9,
        [KeyboardKey.F] = 14,
        [KeyboardKey.Z] = 10,
        [KeyboardKey.X] = 0,
        [KeyboardKey.B] = 11,
        [KeyboardKey.V] = 15,
    };
    int[] Fonts = [
        0xF0, 0x90, 0x90, 0x90, 0xF0,  // 0
        0x20, 0x60, 0x20, 0x20, 0x70,  // 1
        0xF0, 0x10, 0xF0, 0x80, 0xF0,  // 2
        0xF0, 0x10, 0xF0, 0x10, 0xF0,  // 3
        0x90, 0x90, 0xF0, 0x10, 0x10,  // 4
        0xF0, 0x80, 0xF0, 0x10, 0xF8,  // 5
        0xF0, 0x80, 0xF0, 0x90, 0xF0,  // 6
        0xF0, 0x10, 0x20, 0x40, 0x40,  // 7
        0xF0, 0x90, 0xF0, 0x90, 0xF0,  // 8
        0xF0, 0x90, 0xF0, 0x10, 0xF0,  // 9
        0xF0, 0x90, 0xF0, 0x90, 0x90,  // A
        0xE0, 0x90, 0xE0, 0x90, 0xE0,  // B
        0xF0, 0x80, 0x80, 0x80, 0xF0,  // C
        0xE0, 0x90, 0x90, 0x90, 0xE0,  // D
        0xF0, 0x80, 0xF0, 0x80, 0xF0,  // E
        0xF0, 0x80, 0xF0, 0x80, 0x80,  // F
    ];
    static Dictionary<int, System.Action> Opcodes = new Dictionary<int, System.Action>()
    {
        [0x0000] = Opcode0NNN,
        [0x00E0] = Opcode00E0,
        [0x000E] = Opcode00EE,
        [0x1000] = Opcode1NNN,
        [0x2000] = Opcode2NNN,
        [0x3000] = Opcode3XNN,
        [0x4000] = Opcode4XNN,
        [0x5000] = Opcode5XY0,
        [0x6000] = Opcode6XNN,
        [0x7000] = Opcode7XNN,
        [0x8000] = Opcode8XY0,
        [0x8001] = Opcode8XY1,
        [0x8002] = Opcode8XY2,
        [0x8003] = Opcode8XY3,
        [0x8004] = Opcode8XY4,
        [0x8005] = Opcode8XY5,
        [0x8006] = Opcode8XY6,
        [0x8007] = Opcode8XY7,
        [0x800E] = Opcode8XYE,
        [0x9000] = Opcode9XY0,
        [0xA000] = OpcodeANNN,
        [0xB000] = OpcodeBNNN,
        [0xC000] = OpcodeCXNN,
        [0xD000] = OpcodeDXYN,
        [0xE000] = OpcodeENNN,
        [0xE00E] = OpcodeEX9E,
        [0xE001] = OpcodeEXA1,
        [0xF000] = OpcodeFNNN,
        [0xF007] = OpcodeFX07,
        [0xF00A] = OpcodeFX0A,
        [0xF015] = OpcodeFX15,
        [0xF018] = OpcodeFX18,
        [0xF01E] = OpcodeFX1E,
        [0xF029] = OpcodeFX29,
        [0xF033] = OpcodeFX33,
        [0xF055] = OpcodeFX55,
        [0xF065] = OpcodeFX65
    };

    static string LoadRom(string FilePath = @".\roms\pong.ch8") {
        string ROM = FilePath;
        return ROM;
    }

    static void Opcode0NNN() {
        //
    }

    static void Opcode00E0() {
        Raylib.ClearBackground(Color.Black);
    }

    static void Opcode00EE() {
        //
    }
    
    static void Opcode1NNN() {
        //
    }
    
    static void Opcode2NNN() {
        //
    }
    
    static void Opcode3XNN() {
        //
    }

    static void Opcode4XNN() {
        //
    }
    
    static void Opcode5XY0() {
        //
    }

    static void Opcode6XNN() {
        //
    }
    
    static void Opcode7XNN() {
        //
    }

    static void Opcode8XY0() {
        //
    }
    
    static void Opcode8XY1() {
        //
    }

    static void Opcode8XY2() {
        //
    }
    
    static void Opcode8XY3() {
        //
    }

    static void Opcode8XY4() {
        //
    }
    
    static void Opcode8XY5() {
        //
    }

    static void Opcode8XY6() {
        //
    }
    
    static void Opcode8XY7() {
        //
    }

    static void Opcode8XYE() {
        //
    }
    
    static void Opcode9XY0() {
        //
    }

    static void OpcodeANNN() {
        //
    }
    
    static void OpcodeBNNN() {
        //
    }

    static void OpcodeCXNN() {
        //
    }
    
    static void OpcodeDXYN() {
        //
    }

    static void OpcodeENNN() {
        //
    }

    static void OpcodeEX9E() {
        //
    }

    static void OpcodeEXA1() {
        //
    }

    static void OpcodeFNNN() {
        //
    }
    
    static void OpcodeFX07() {
        //
    }

    static void OpcodeFX0A() {
        //
    }

    static void OpcodeFX15() {
        //
    }
    
    static void OpcodeFX18() {
        //
    }

    static void OpcodeFX1E() {
        //
    }
    
    static void OpcodeFX29() {
        //
    }

    static void OpcodeFX33() {
        //
    }
    
    static void OpcodeFX55() {
        //
    }

    static void OpcodeFX65() {
        //
    }

    public static void Main() {
        Raylib.InitWindow(WindowSize[0], WindowSize[1], Path.GetFileName(LoadRom()));

        Raylib.SetTargetFPS(60);
        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            // Raylib.ClearBackground(Color.Black);

            Raylib.EndDrawing();
        }
        Raylib.CloseWindow();
    }
}
*/
