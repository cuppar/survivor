using Godot;
#if IMGUI
using ImGuiGodot;
using ImGuiNET;
#endif

namespace Survivor;

public partial class Test : Node2D
{
    public override void _Ready()
    {
        base._Ready();
#if IMGUI
        ImGuiGD.Connect(OnImGuiLayout);
#endif
    }

#if IMGUI
    private void OnImGuiLayout()
    {
        ImGui.Begin("ImGui on Godot 4");
        ImGui.Text("hello world");
        ImGui.End();
    }
#endif
}