using ActionTimelineReplacement.Configurations;
using ActionTimelineReplacement.Hookers;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Interface;
using System.Numerics;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using Dalamud.Interface.Utility;

namespace ActionTimelineReplacement.Windows.SubSheets
{
    public class PetMirageSubSheet
    {
        private static float Scale => ImGuiHelpers.GlobalScale;
        public static void Draw(string mainkey, ref Configuration.ReplacementSet _activeSet, ref Dictionary<string, float>  _AllItemWidths,ref string _searchPetMirage)
        {
            using (var subList = ImRaii.Child(mainkey + "SubList", new Vector2(-1f, (float)(_activeSet.PetMirageReplacements.Count() * 30) + 30), false))
            {
                if (subList)
                {
                    foreach (var key in _activeSet.PetMirageReplacements.Keys)
                    {   //SPECIFICSTART
                        var replacement = _activeSet.PetMirageReplacements[key].Replacement;

                        if (ImGui.Checkbox("##" + key, ref _activeSet.PetMirageReplacements[key].Enabled))
                        {
                            Methods.SetupPetMirage(key);
                            Service.Config.Save();
                        }
                        ImGui.SameLine();
                        if (ImGui.Button(" - ##" + key))
                        {
                            _activeSet.PetMirageReplacements.Remove(key);
                        }
                        //SPECIFICEND
                        ImGui.SameLine();
                        ImGui.Text($"#{key:D5}");

                        ImGui.SameLine();
                        if (_AllItemWidths[mainkey] != 0 && Service.Config.AdvancedMode)
                        {
                            var widthRest = ImGui.GetWindowWidth() - _AllItemWidths[mainkey] - ImGui.GetCursorPosX() - 5 * Scale;
                            ImGui.PushTextWrapPos(Math.Max(widthRest, 60 * Scale) + ImGui.GetCursorPosX());
                        }
                        ImGui.TextWrapped(PetMirageReplacementsManager.GetName(key));
                        if (_AllItemWidths[mainkey] != 0 && Service.Config.AdvancedMode)
                        {
                            ImGui.PopTextWrapPos();
                        }

                        if (!Service.Config.AdvancedMode)
                        {
                            continue;
                        }

                        ImGui.SameLine();
                        if (_AllItemWidths[mainkey] != 0)
                        {
                            ImGui.SetCursorPosX(ImGui.GetWindowWidth() - _AllItemWidths[mainkey]);
                        }

                        var startwidth = ImGui.GetCursorPosX();

                        /*DrawItemString("Name", ref replacement.petMirageName, i => i.petMirageName);
                        ImGui.SameLine();*/

                        DrawItem("Unk0", ref replacement.petMirageUnk0, i => i.petMirageUnk0);
                        ImGui.SameLine();

                        DrawItem("Unk1", ref replacement.petMirageUnk1, i => i.petMirageUnk1);
                        ImGui.SameLine();

                        DrawItem("Unk2", ref replacement.petMirageUnk2, i => i.petMirageUnk2);
                        ImGui.SameLine();

                        DrawItemByte("Unk3", ref replacement.petMirageUnk3, i => i.petMirageUnk3);
                        ImGui.SameLine();

                        DrawItem("Unk4", ref replacement.petMirageUnk4, i => i.petMirageUnk4);
                        ImGui.SameLine();

                        DrawItem("Unk5", ref replacement.petMirageUnk5, i => i.petMirageUnk5);
                        ImGui.SameLine();

                        DrawItem("Unk6", ref replacement.petMirageUnk6, i => i.petMirageUnk6);
                        ImGui.SameLine();

                        DrawItemByte("Unk7", ref replacement.petMirageUnk7, i => i.petMirageUnk7);
                        ImGui.SameLine();

                        DrawItemFloat("Scale", ref replacement.petMirageScale, i => i.petMirageScale);
                        ImGui.SameLine();

                        /*DrawItem("Model", ref replacement.petMirageModelChara, i => i.petMirageModelChara);
                        ImGui.SameLine();*/

                        ImGui.SameLine();
                        _AllItemWidths[mainkey] = ImGui.GetCursorPosX() - startwidth;
                        ImGui.NewLine();
                        continue;
                        //SPECIFICSTART
                        void DrawItem(string name, ref ushort value,
                            Func<PetMirageReplacement, ushort> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            int relay = value;
                            if (ImGui.DragInt("##" + name + key, ref relay))
                            {
                                value = (ushort)relay;
                                Methods.SetupPetMirage(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(PetMirageReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupPetMirage(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        void DrawItemByte(string name, ref byte value,
                             Func<PetMirageReplacement, byte> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            int relay = value;
                            if (ImGui.DragInt("##" + name + key, ref relay))
                            {
                                value = (byte)relay;
                                Methods.SetupPetMirage(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(PetMirageReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupPetMirage(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        void DrawItemFloat(string name, ref float value,
                             Func<PetMirageReplacement, float> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            float relay = value;
                            if (ImGui.DragFloat("##" + name + key, ref relay))
                            {
                                value = (float)relay;
                                Methods.SetupPetMirage(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(PetMirageReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupPetMirage(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        void DrawItemString(string name, ref string value,
                            Func<PetMirageReplacement, string> getDefault)
                        {
                            ImGui.SetNextItemWidth(60 * Scale);
                            string relayString = value;
                            if (ImGui.InputText("##" + name + key, ref relayString, 260))
                            {
                                value = relayString;
                                Methods.SetupPetMirage(key);
                                Service.Config.Save();
                            }

                            ImGui.SameLine();
                            using (ImRaii.PushFont(UiBuilder.IconFont))
                            {
                                if (ImGui.Button($"{FontAwesomeIcon.Reply.ToIconString()}##{name}{key}"))
                                {
                                    value = getDefault(PetMirageReplacementsManager.GetOriginalReplacement(key));
                                    Methods.SetupPetMirage(key);
                                    Service.Config.Save();
                                }
                            }
                        }
                        //SPECIFICEND
                    }
                    //SPECIFICSTART
                    const string searchPetMiragesPopup = "Search pet mirages";
                    if (ImGui.Button(" + "))
                    {
                        ImGui.OpenPopup(searchPetMiragesPopup);
                    }

                    using var searchPopup = ImRaii.Popup(searchPetMiragesPopup);
                    if (searchPopup)
                    {
                        var width = 200 * Scale;

                        ImGui.SetNextItemWidth(width);
                        ImGui.InputText("##Search pet mirage", ref _searchPetMirage, 256);
                        var localsearch = _searchPetMirage;

                        using var popUpChild = ImRaii.Child(searchPetMiragesPopup, new Vector2(width, 200 * Scale), true);
                        foreach (var pair in PetMirageReplacementsManager.PetMirageNames.OrderBy(i =>
                        {
                            if (string.IsNullOrEmpty(localsearch)) return 0;
                            return Math.Min(ConfigWindow.ScoreString(i.Value, localsearch),
                                ConfigWindow.ScoreString(i.Key.ToString(), localsearch));
                        }))
                        {
                            if (ImGui.Selectable($"#{pair.Key:D5} {pair.Value}"))
                            {
                                var original = PetMirageReplacementsManager.GetOriginalReplacement(pair.Key);
                                _activeSet.PetMirageReplacements[pair.Key] =
                                    new PetMirageReplacementConfig(new PetMirageReplacement(
                                            //original.petMirageName,
                                            original.petMirageUnk0,
                                            original.petMirageUnk1,
                                            original.petMirageUnk2,
                                            original.petMirageUnk3,
                                            original.petMirageUnk4,
                                            original.petMirageUnk5,
                                            original.petMirageUnk6,
                                            original.petMirageUnk7,
                                            original.petMirageScale
                                            //original.petMirageModelChara
                                            ),
                                        false);
                                Service.Config.Save();
                            }
                        }
                    }
                    //SPECIFICEND
                }
            }
        }
    }
}
