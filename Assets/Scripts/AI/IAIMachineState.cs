﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    interface IAIMachineState
    {
        void Enter();
        void Pause();
        void Stop();
        event Action OnComplete;
    }
}