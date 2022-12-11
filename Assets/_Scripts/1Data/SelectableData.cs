using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets._Scripts._1Data
{
    public abstract class SelectableData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Transform ParentTransform { get; set; }
    }
}
