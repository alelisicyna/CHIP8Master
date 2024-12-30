﻿using System;
using System.IO; 
using System.Text; 
using System.Numerics;
using Raylib_cs;

class Emulator {
    // Size of window
    static int Increase = 10;
    static int[] WindowSize = [64 * Increase, 32 * Increase];

    // CPU
    public static byte[] Memory = new byte[4096];
    public static int PC = 0;
    public static ushort[] Registers = new ushort[16];
    public static int I = 0;
    public static ushort[] Stack = new ushort[16];
    public static int SP = 0;
    public static int SoundTimer = 0;
    public static int DelayTimer  = 0;
    public static byte[] ScreenBuffer = new byte[32 * 64];
    public static int FPS = 60;
    public static int Opcode = 0;

    public static bool IsDrawing = true;

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
    public static byte[] Fonts = [
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

    public void Initialize() {
        PC = 512;
        for (int i = 0; i < Fonts.Length /* 80 */; i++) {
            Memory[i] = Fonts[i];
        }
    }

    public void LoadRom(string FilePath) {
        var CHIPFile = File.ReadAllBytes(FilePath);
        for (int i = 0; i < CHIPFile.Length; i++) {
            Memory[i + 0x200] = CHIPFile[i];
        }
    }

    static void Opcode0NNN() {
        // Calls machine code routine (RCA 1802 for COSMAC VIP) at address NNN.
        // Not necessary for most ROMs.

        if (Opcode == 0x0) {
            Raylib.CloseWindow();
        }
        if (Opcode == 0xe0) {
            Opcode00E0();
            return;
        }

        try {
            Opcodes[Opcode & 0xf00f]();
        } catch {
            PC = PC + 2;
        }
    }

    static void Opcode00E0() {
        // Clears the screen.

        ScreenBuffer = new byte[32 * 64];
        IsDrawing = true;
        PC = PC + 2;
    }

    static void Opcode00EE() {
        // Returns from a subroutine.

        PC = Stack[SP] + 2;
        SP = SP - 1;
    }
    
    static void Opcode1NNN() {
        // Jumps to address NNN.

        PC = Opcode & 0x0FFF;
    }
    
    static void Opcode2NNN() {
        // Calls subroutine at NNN.
    }
    
    static void Opcode3XNN() {
        // Skips the next instruction if VX equals NN (usually the next instruction is a jump to skip a code block).
    }

    static void Opcode4XNN() {
        // Skips the next instruction if VX does not equal NN (usually the next instruction is a jump to skip a code block).
    }
    
    static void Opcode5XY0() {
        // Skips the next instruction if VX equals VY (usually the next instruction is a jump to skip a code block).
    }

    static void Opcode6XNN() {
        // Sets VX to NN.
    }
    
    static void Opcode7XNN() {
        // Adds NN to VX (carry flag is not changed).
    }

    static void Opcode8XY0() {
        // Sets VX to the value of VY.
    }
    
    static void Opcode8XY1() {
        // Sets VX to VX or VY. (bitwise OR operation).
    }

    static void Opcode8XY2() {
        // Sets VX to VX and VY. (bitwise AND operation).
    }
    
    static void Opcode8XY3() {
        // Sets VX to VX xor VY.
    }

    static void Opcode8XY4() {
        // Adds VY to VX. VF is set to 1 when there's an overflow, and to 0 when there is not.
    }
    
    static void Opcode8XY5() {
        // VY is subtracted from VX. VF is set to 0 when there's an underflow, and 1 when there is not. (i.e. VF set to 1 if VX >= VY and 0 if not).
    }

    static void Opcode8XY6() {
        // Shifts VX to the right by 1, then stores the least significant bit of VX prior to the shift into VF.
    }
    
    static void Opcode8XY7() {
        // Sets VX to VY minus VX. VF is set to 0 when there's an underflow, and 1 when there is not. (i.e. VF set to 1 if VY >= VX).
    }

    static void Opcode8XYE() {
        // Shifts VX to the left by 1, then sets VF to 1 if the most significant bit of VX prior to that shift was set, or to 0 if it was unset.
    }
    
    static void Opcode9XY0() {
        // Skips the next instruction if VX does not equal VY.
        // (Usually the next instruction is a jump to skip a code block).
    }

    static void OpcodeANNN() {
        // Sets I to the address NNN.
    }
    
    static void OpcodeBNNN() {
        // Jumps to the address NNN plus V0.
    }

    static void OpcodeCXNN() {
        // Sets VX to the result of a bitwise and operation on a random number (Typically: 0 to 255) and NN.
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

    static void DrawScreen(Color BackgroundColor, Color ColorPixels) {
        Raylib.ClearBackground(Color.Black);
        for (int i = 0; i < ScreenBuffer.Length; i++) {
            if (ScreenBuffer[i] == 1) {
                Raylib.DrawRectangle((i % 64) * Increase, (i / 64) * Increase, Increase, Increase, ColorPixels);
            } else {
                Raylib.DrawRectangle((i % 64) * Increase, (i / 64) * Increase, Increase, Increase, BackgroundColor);
            }
        }
    }

    // Close CHIP-8 window
    public void Close() {
        Raylib.CloseWindow();
    }

    // The main method
    public void Run(string Directory, string FileName, Color BackgroundColor, Color ColorPixels) {
        string FilePath = $"{Directory}{FileName}";
        Raylib.InitWindow(WindowSize[0], WindowSize[1], $"PongEmu - {FileName}");

        // Emulation preparation
        Initialize();
        LoadRom(FilePath);

        Raylib.SetTargetFPS(FPS);
        while (!Raylib.WindowShouldClose()) {
            Raylib.BeginDrawing();
            if (IsDrawing) DrawScreen(BackgroundColor, ColorPixels);
            Raylib.EndDrawing();
        }
        
        Raylib.CloseWindow();
    }
}
