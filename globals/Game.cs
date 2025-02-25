﻿using System;

namespace Survivor.Globals;

public static class Game
{
    public static event Action<float>? ShakeCameraEvent;

    public static void ShakeCamera(float amount)
    {
        ShakeCameraEvent?.Invoke(amount);
    }
}