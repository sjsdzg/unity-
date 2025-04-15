using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFramework.Core;
using UnityEngine;

namespace XFramework.Architectural
{
    public class ArchitectSettings
    {
        public static class Depth2D
        {
            public static int corner = 0;
            public static int wall = 1;
            public static int room = 2;
            public static int door = 3;
            public static int window = 3;
            public static int pass = 3;
        }

        public static class ColorBlock
        {
            public static PrefColor highlightedColor = new PrefColor("architect.colorblock.highlightedColor", new Color32(25, 125, 255, 255));
            public static PrefColor selectedColor = new PrefColor("architect.colorblock.selectedColor", new Color32(25, 125, 255, 255));
        }

        public static class Assist
        {
            public static PrefColor color = new PrefColor("architect.assist.color", new Color32(0, 255, 0, 255));
            public static PrefColor warningColor = new PrefColor("architect.assist.warningColor", Color.red);
            public static PrefMaterial material = new PrefMaterial("architect.assist.material", "Architect/Materials/Assist");
            public static PrefColor xAxisColor = new PrefColor("architect.assist.xAxisColor", new Color(219f / 255, 62f / 255, 29f / 255, .93f));
            public static PrefColor yAxisColor = new PrefColor("architect.assist.yAxisColor", new Color(154f / 255, 243f / 255, 72f / 255, .93f));
            public static PrefColor zAxisColor = new PrefColor("architect.assist.zAxisColor", new Color(58f / 255, 122f / 255, 248f / 255, .93f));
        }

        public static class Wall
        {
            public static PrefBool ortho = new PrefBool("architect.wall.ortho", false);
            public static PrefFloat thickness = new PrefFloat("architect.wall.thickness", 0.15f);
            public static PrefColor color2d = new PrefColor("architect.wall.color2d", new Color32(255, 255, 255, 255));
            public static PrefMaterial material2d = new PrefMaterial("architect.wall.material2d", "Architect/Materials/Wall2D");
            public static PrefColor color3d = new PrefColor("architect.wall.color3d", new Color32(255, 255, 255, 255));
            public static PrefMaterial material3d = new PrefMaterial("architect.wall.material3d", "Architect/Materials/Wall3D");
        }

        public static class Corner
        {
            public static PrefColor color2d = new PrefColor("architect.corner.color2d", new Color32(255, 255, 255, 153));
            public static PrefMaterial material2d = new PrefMaterial("architect.wall.material2d", "Architect/Materials/Corner2D");
            public static PrefTexture2D texture = new PrefTexture2D("architect.corner.texture", "Architect/Textures/Corner");
        }

        public static class Door
        {
            public static PrefFloat length = new PrefFloat("architect.door.length", 0.9f);
            public static PrefFloat double_length = new PrefFloat("architect.door.double_length", 1.5f);
            public static PrefFloat revolving_length = new PrefFloat("architect.door.revolving_length", 3f);
            public static PrefFloat height = new PrefFloat("architect.door.height", 2.1f);
            public static PrefFloat thickness = new PrefFloat("architect.door.thickness", 0.15f);
            public static PrefFloat bottom = new PrefFloat("architect.door.bottom", 0f);
            public static PrefColor color2d = new PrefColor("architect.door.color2d", new Color32(0, 255, 255, 255));
            public static PrefMaterial material2d = new PrefMaterial("architect.wall.material2d", "Architect/Materials/Door2D");
        }

        public static class Window
        {
            public static PrefFloat length = new PrefFloat("architect.window.length", 1.2f);
            public static PrefFloat height = new PrefFloat("architect.window.height", 0.9f);
            public static PrefFloat thickness = new PrefFloat("architect.window.thickness", 0.15f);
            public static PrefFloat bottom = new PrefFloat("architect.window.bottom", 0.9f);
            public static PrefColor color2d = new PrefColor("architect.window.color2d", new Color32(0, 255, 255, 255));
            public static PrefMaterial material2d = new PrefMaterial("architect.window.material2d", "Architect/Materials/Window2D");
        }

        public static class Pass
        {
            public static PrefFloat length = new PrefFloat("architect.window.length", 1.1f);
            public static PrefFloat height = new PrefFloat("architect.window.height", 2.1f);
            public static PrefFloat thickness = new PrefFloat("architect.window.thickness", 0.12f);
            public static PrefFloat bottom = new PrefFloat("architect.window.bottom", 0f);
            public static PrefColor color2d = new PrefColor("architect.window.color2d", new Color32(0, 255, 255, 255));
            public static PrefMaterial material2d = new PrefMaterial("architect.window.material2d", "Architect/Materials/Pass2D");
        }

        public static class Room
        {
            public static PrefColor color2d = new PrefColor("architect.room.color2d", new Color32(255, 255, 255, 25));
            public static PrefMaterial material2d = new PrefMaterial("architect.room.material2d", "Architect/Materials/Room2D");
            public static PrefColor color3d = new PrefColor("architect.room.color3d", new Color32(255, 255, 255, 255));
            public static PrefMaterial material3d = new PrefMaterial("architect.room.material3d", "Architect/Materials/Room3D");
        }

        public static class Area
        {
            public static PrefColor color2d = new PrefColor("architect.area.color2d", new Color32(255, 255, 255, 25));
            public static PrefColor outlineColor2d = new PrefColor("architect.area.outlineColor2d", new Color32(255, 255, 255, 255));
            public static PrefMaterial material2d = new PrefMaterial("architect.area.material2d", "Architect/Materials/Area2D");
            public static PrefColor color3d = new PrefColor("architect.area.color3d", new Color32(255, 255, 255, 255));
            public static PrefMaterial material3d = new PrefMaterial("architect.area.material3d", "Architect/Materials/Area3D");
        }
    }
}
