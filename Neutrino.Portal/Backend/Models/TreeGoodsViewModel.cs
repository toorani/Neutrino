using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Espresso.Core;
using Espresso.Portal;

namespace Neutrino.Portal.Models
{
    public class NodeState
    {
        public bool Opened { get; set; }
        public bool Disabled { get; set; }
        public bool Selected { get; set; }
        public NodeState()
        {
            Opened = false;
            Disabled = false;
            Selected = false;
        }
    }
    public class jsTreeViewModel  
    {
        public int Id { get; set; }
        public string EnName { get; set; }
        public string Text { get; set; }
        public string Code { get; set; }
        public string Parent { get; set; }
        public bool Children { get; set; }
        public NodeState State { get; set; }

        public jsTreeViewModel() 
        {
            State = new NodeState();
            Children = true;
        }

    }
    public class jsTreeStaticViewModel
    {
        public string Id { get; set; }
        public string EnName { get; set; }
        public string ExtraData { get; set; }
        public string Text { get; set; }
        public string Parent { get; set; }
        public NodeState State { get; set; }

        public jsTreeStaticViewModel()
        {
            State = new NodeState();
        }

    }

}