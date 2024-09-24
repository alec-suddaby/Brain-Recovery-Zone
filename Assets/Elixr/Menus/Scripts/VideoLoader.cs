using Elixr.MenuSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoLoader : LoadLevelNode
{
    [SerializeField] private List<VideoDescription> videos;
    public List<VideoDescription> Videos => videos;
}
