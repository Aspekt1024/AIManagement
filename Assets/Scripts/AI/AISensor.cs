﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aspekt.AI
{
    public abstract class AISensor : MonoBehaviour
    {
        protected AIAgent agent;

        // TODO states and update frequency (turn on/off etc)

        public AISensor(AIAgent agent)
        {
            this.agent = agent;
        }
    }
}
