﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetActorsClientTests
{
    /// <summary>
    /// Defines the reply message that represents an error.
    /// </summary>
    public class ActorException
    {
        public string Error { get; set; }
        public string Exception { get; set; }
    }
}
