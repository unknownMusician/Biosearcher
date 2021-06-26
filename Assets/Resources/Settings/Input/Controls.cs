// GENERATED AUTOMATICALLY FROM 'Assets/Resources/Settings/Input/Controls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Controls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Controls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Controls"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""f43be611-ce0e-4bf0-aaa9-12c5f792d077"",
            ""actions"": [
                {
                    ""name"": ""TangentAccelerationStart"",
                    ""type"": ""Value"",
                    ""id"": ""2ab5dc8e-ac50-4432-a79c-c3e3b50f3601"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""TangentAccelerationStop"",
                    ""type"": ""Value"",
                    ""id"": ""e3b27190-3601-480a-b75e-f67149f77731"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NormalAccelerationStart"",
                    ""type"": ""Value"",
                    ""id"": ""6d8caad1-d4a9-40b5-9ffd-c10f5b768c75"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""NormalAccelerationStop"",
                    ""type"": ""Button"",
                    ""id"": ""5d85ce9f-b046-40a2-8b0e-3b394c54f90f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WS"",
                    ""id"": ""3c52061b-c176-4e71-8bb5-515e46d2d710"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TangentAccelerationStart"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""05ee9040-9657-4fa0-b86d-8711641be370"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""TangentAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""3dbe0b39-f63d-4a73-b497-28e48c84a96e"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""TangentAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Triggers"",
                    ""id"": ""eaacb994-220d-4bd5-a3ab-bdd1ffc3b59a"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TangentAccelerationStart"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""cae67df3-ef39-433f-8b6f-ba7d1a7d4017"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TangentAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""591e727e-f16d-4590-ab6b-17a3100f1af7"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TangentAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""WS"",
                    ""id"": ""c074c63f-bad5-49e4-a4e3-2c3939d29fbf"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TangentAccelerationStop"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""5165c1e0-fd12-448e-a7ba-74c64bb88531"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""TangentAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""8b5d453b-c18d-44b5-a8be-555d689f1a6f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""TangentAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Triggers"",
                    ""id"": ""bc5e9827-faf6-4a16-985d-d455e1450d2f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TangentAccelerationStop"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""14c4a7b0-4121-4b3d-9411-f72b5663f9a8"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TangentAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""584839ba-d45e-4eba-bbc0-a9b926fe95da"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""TangentAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DA"",
                    ""id"": ""71a528ca-8e41-4ea7-8610-fcde2ff850a2"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NormalAccelerationStart"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""17dfac79-f738-4c9a-9fc0-c0d9fe85511f"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""NormalAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""03da9f3c-da0c-49ec-9c1a-2fa4ef4a2ee7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""NormalAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""1a64829e-7c15-490d-9fc0-25714f826aa6"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NormalAccelerationStart"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""40a7d6ae-5656-4aeb-8403-d1b4445ffa83"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NormalAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""635d554c-3956-4339-a5c2-bfb9742549ff"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NormalAccelerationStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""DA"",
                    ""id"": ""d3ced78b-d26e-4407-9e50-3afdbe5e8b1f"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NormalAccelerationStop"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""8c733539-b4dc-44f1-80e5-f564098d1534"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""NormalAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""7eca10ad-3b3d-48b1-b354-8ebc57ec2758"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""NormalAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left Stick"",
                    ""id"": ""34987a4a-83fc-4b04-b023-1968e173eea4"",
                    ""path"": ""1DAxis"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NormalAccelerationStop"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ddbdc311-d383-4448-9004-2d0ca3dba1c0"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NormalAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""1cd7784c-2f7d-4b2a-86d7-ca345655a5f4"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""NormalAccelerationStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""Camera"",
            ""id"": ""52c53913-be3e-44ef-afcf-72aca7e5d177"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""7e315d8b-de14-4c20-bb82-73c2a83e09bb"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveStart"",
                    ""type"": ""Value"",
                    ""id"": ""98479d87-c39c-40f4-966f-7f1b9dc3d1c4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""MoveStop"",
                    ""type"": ""Value"",
                    ""id"": ""146d9db5-88e6-4266-a03b-6d2127f98563"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""InteractionStateChange"",
                    ""type"": ""Button"",
                    ""id"": ""004f7f26-3d23-4920-a5f3-b1857e153013"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""bcbf0a7f-a190-40fc-85ec-032558e05020"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40d3ba1c-95c7-4f70-b41f-423c4931814d"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8482d2c2-a108-49af-8c78-cf0179db36c8"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""78d93ab1-bb81-4aad-8f7f-5206b5bcdccd"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""MoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b40ad086-2346-4ddd-8eaf-a8bfcd035ed2"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""InteractionStateChange"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Terraformer"",
            ""id"": ""3d98999e-086f-41a5-96a5-3492344efe9a"",
            ""actions"": [
                {
                    ""name"": ""AddStart"",
                    ""type"": ""Button"",
                    ""id"": ""d80edf32-8e44-4d94-a989-d0572fd49380"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""AddStop"",
                    ""type"": ""Button"",
                    ""id"": ""6d65461e-4164-40d2-8e5d-88ea4f233011"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveStart"",
                    ""type"": ""Button"",
                    ""id"": ""899306e4-0214-41b2-ae2b-ac1b52646622"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""RemoveStop"",
                    ""type"": ""Button"",
                    ""id"": ""3dcba410-13b2-4276-9bca-944d606b34e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""70798630-7616-42fb-90df-4dee8c3407c9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""AddStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""45dc5adc-2bb6-4b2a-9636-4e21593ada10"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AddStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""324c4b31-7550-4745-b74a-65f0a33a336b"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""AddStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4e6668df-c7a4-449b-9f6c-a3f943a4064f"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""AddStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""155a6940-d7f5-48a1-961b-0e0f23f5ca85"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""RemoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""951743ef-c553-4c49-978f-5d4bc5a95c3c"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RemoveStop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""1196c8ba-75f0-4939-87c5-383049439ac9"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Keyboard and Mouse"",
                    ""action"": ""RemoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""750326f6-103a-4481-88ed-c76b548566d3"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""RemoveStart"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Grabber"",
            ""id"": ""6dcb8437-3419-4b91-a046-d7d90ff92c0f"",
            ""actions"": [
                {
                    ""name"": ""Drop"",
                    ""type"": ""Button"",
                    ""id"": ""e52e1742-99e5-411d-98ba-78f4b440f8cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Grab"",
                    ""type"": ""Button"",
                    ""id"": ""a11032e3-80f3-495f-99bf-1b683347cae5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8acc2125-d32f-4064-b105-8d95409fcc05"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Grab"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d8eb4733-e82d-4864-9e13-9b5604db94bd"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": ""Press(behavior=1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Drop"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard and Mouse"",
            ""bindingGroup"": ""Keyboard and Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_TangentAccelerationStart = m_Player.FindAction("TangentAccelerationStart", throwIfNotFound: true);
        m_Player_TangentAccelerationStop = m_Player.FindAction("TangentAccelerationStop", throwIfNotFound: true);
        m_Player_NormalAccelerationStart = m_Player.FindAction("NormalAccelerationStart", throwIfNotFound: true);
        m_Player_NormalAccelerationStop = m_Player.FindAction("NormalAccelerationStop", throwIfNotFound: true);
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_Move = m_Camera.FindAction("Move", throwIfNotFound: true);
        m_Camera_MoveStart = m_Camera.FindAction("MoveStart", throwIfNotFound: true);
        m_Camera_MoveStop = m_Camera.FindAction("MoveStop", throwIfNotFound: true);
        m_Camera_InteractionStateChange = m_Camera.FindAction("InteractionStateChange", throwIfNotFound: true);
        // Terraformer
        m_Terraformer = asset.FindActionMap("Terraformer", throwIfNotFound: true);
        m_Terraformer_AddStart = m_Terraformer.FindAction("AddStart", throwIfNotFound: true);
        m_Terraformer_AddStop = m_Terraformer.FindAction("AddStop", throwIfNotFound: true);
        m_Terraformer_RemoveStart = m_Terraformer.FindAction("RemoveStart", throwIfNotFound: true);
        m_Terraformer_RemoveStop = m_Terraformer.FindAction("RemoveStop", throwIfNotFound: true);
        // Grabber
        m_Grabber = asset.FindActionMap("Grabber", throwIfNotFound: true);
        m_Grabber_Drop = m_Grabber.FindAction("Drop", throwIfNotFound: true);
        m_Grabber_Grab = m_Grabber.FindAction("Grab", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Player
    private readonly InputActionMap m_Player;
    private IPlayerActions m_PlayerActionsCallbackInterface;
    private readonly InputAction m_Player_TangentAccelerationStart;
    private readonly InputAction m_Player_TangentAccelerationStop;
    private readonly InputAction m_Player_NormalAccelerationStart;
    private readonly InputAction m_Player_NormalAccelerationStop;
    public struct PlayerActions
    {
        private @Controls m_Wrapper;
        public PlayerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @TangentAccelerationStart => m_Wrapper.m_Player_TangentAccelerationStart;
        public InputAction @TangentAccelerationStop => m_Wrapper.m_Player_TangentAccelerationStop;
        public InputAction @NormalAccelerationStart => m_Wrapper.m_Player_NormalAccelerationStart;
        public InputAction @NormalAccelerationStop => m_Wrapper.m_Player_NormalAccelerationStop;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterface != null)
            {
                @TangentAccelerationStart.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTangentAccelerationStart;
                @TangentAccelerationStart.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTangentAccelerationStart;
                @TangentAccelerationStart.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTangentAccelerationStart;
                @TangentAccelerationStop.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTangentAccelerationStop;
                @TangentAccelerationStop.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTangentAccelerationStop;
                @TangentAccelerationStop.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnTangentAccelerationStop;
                @NormalAccelerationStart.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAccelerationStart;
                @NormalAccelerationStart.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAccelerationStart;
                @NormalAccelerationStart.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAccelerationStart;
                @NormalAccelerationStop.started -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAccelerationStop;
                @NormalAccelerationStop.performed -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAccelerationStop;
                @NormalAccelerationStop.canceled -= m_Wrapper.m_PlayerActionsCallbackInterface.OnNormalAccelerationStop;
            }
            m_Wrapper.m_PlayerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @TangentAccelerationStart.started += instance.OnTangentAccelerationStart;
                @TangentAccelerationStart.performed += instance.OnTangentAccelerationStart;
                @TangentAccelerationStart.canceled += instance.OnTangentAccelerationStart;
                @TangentAccelerationStop.started += instance.OnTangentAccelerationStop;
                @TangentAccelerationStop.performed += instance.OnTangentAccelerationStop;
                @TangentAccelerationStop.canceled += instance.OnTangentAccelerationStop;
                @NormalAccelerationStart.started += instance.OnNormalAccelerationStart;
                @NormalAccelerationStart.performed += instance.OnNormalAccelerationStart;
                @NormalAccelerationStart.canceled += instance.OnNormalAccelerationStart;
                @NormalAccelerationStop.started += instance.OnNormalAccelerationStop;
                @NormalAccelerationStop.performed += instance.OnNormalAccelerationStop;
                @NormalAccelerationStop.canceled += instance.OnNormalAccelerationStop;
            }
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_Move;
    private readonly InputAction m_Camera_MoveStart;
    private readonly InputAction m_Camera_MoveStop;
    private readonly InputAction m_Camera_InteractionStateChange;
    public struct CameraActions
    {
        private @Controls m_Wrapper;
        public CameraActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Camera_Move;
        public InputAction @MoveStart => m_Wrapper.m_Camera_MoveStart;
        public InputAction @MoveStop => m_Wrapper.m_Camera_MoveStop;
        public InputAction @InteractionStateChange => m_Wrapper.m_Camera_InteractionStateChange;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMove;
                @MoveStart.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStart;
                @MoveStart.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStart;
                @MoveStart.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStart;
                @MoveStop.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStop;
                @MoveStop.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStop;
                @MoveStop.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnMoveStop;
                @InteractionStateChange.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnInteractionStateChange;
                @InteractionStateChange.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnInteractionStateChange;
                @InteractionStateChange.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnInteractionStateChange;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @MoveStart.started += instance.OnMoveStart;
                @MoveStart.performed += instance.OnMoveStart;
                @MoveStart.canceled += instance.OnMoveStart;
                @MoveStop.started += instance.OnMoveStop;
                @MoveStop.performed += instance.OnMoveStop;
                @MoveStop.canceled += instance.OnMoveStop;
                @InteractionStateChange.started += instance.OnInteractionStateChange;
                @InteractionStateChange.performed += instance.OnInteractionStateChange;
                @InteractionStateChange.canceled += instance.OnInteractionStateChange;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);

    // Terraformer
    private readonly InputActionMap m_Terraformer;
    private ITerraformerActions m_TerraformerActionsCallbackInterface;
    private readonly InputAction m_Terraformer_AddStart;
    private readonly InputAction m_Terraformer_AddStop;
    private readonly InputAction m_Terraformer_RemoveStart;
    private readonly InputAction m_Terraformer_RemoveStop;
    public struct TerraformerActions
    {
        private @Controls m_Wrapper;
        public TerraformerActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @AddStart => m_Wrapper.m_Terraformer_AddStart;
        public InputAction @AddStop => m_Wrapper.m_Terraformer_AddStop;
        public InputAction @RemoveStart => m_Wrapper.m_Terraformer_RemoveStart;
        public InputAction @RemoveStop => m_Wrapper.m_Terraformer_RemoveStop;
        public InputActionMap Get() { return m_Wrapper.m_Terraformer; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TerraformerActions set) { return set.Get(); }
        public void SetCallbacks(ITerraformerActions instance)
        {
            if (m_Wrapper.m_TerraformerActionsCallbackInterface != null)
            {
                @AddStart.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStart;
                @AddStart.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStart;
                @AddStart.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStart;
                @AddStop.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStop;
                @AddStop.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStop;
                @AddStop.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnAddStop;
                @RemoveStart.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStart;
                @RemoveStart.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStart;
                @RemoveStart.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStart;
                @RemoveStop.started -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStop;
                @RemoveStop.performed -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStop;
                @RemoveStop.canceled -= m_Wrapper.m_TerraformerActionsCallbackInterface.OnRemoveStop;
            }
            m_Wrapper.m_TerraformerActionsCallbackInterface = instance;
            if (instance != null)
            {
                @AddStart.started += instance.OnAddStart;
                @AddStart.performed += instance.OnAddStart;
                @AddStart.canceled += instance.OnAddStart;
                @AddStop.started += instance.OnAddStop;
                @AddStop.performed += instance.OnAddStop;
                @AddStop.canceled += instance.OnAddStop;
                @RemoveStart.started += instance.OnRemoveStart;
                @RemoveStart.performed += instance.OnRemoveStart;
                @RemoveStart.canceled += instance.OnRemoveStart;
                @RemoveStop.started += instance.OnRemoveStop;
                @RemoveStop.performed += instance.OnRemoveStop;
                @RemoveStop.canceled += instance.OnRemoveStop;
            }
        }
    }
    public TerraformerActions @Terraformer => new TerraformerActions(this);

    // Grabber
    private readonly InputActionMap m_Grabber;
    private IGrabberActions m_GrabberActionsCallbackInterface;
    private readonly InputAction m_Grabber_Drop;
    private readonly InputAction m_Grabber_Grab;
    public struct GrabberActions
    {
        private @Controls m_Wrapper;
        public GrabberActions(@Controls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Drop => m_Wrapper.m_Grabber_Drop;
        public InputAction @Grab => m_Wrapper.m_Grabber_Grab;
        public InputActionMap Get() { return m_Wrapper.m_Grabber; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GrabberActions set) { return set.Get(); }
        public void SetCallbacks(IGrabberActions instance)
        {
            if (m_Wrapper.m_GrabberActionsCallbackInterface != null)
            {
                @Drop.started -= m_Wrapper.m_GrabberActionsCallbackInterface.OnDrop;
                @Drop.performed -= m_Wrapper.m_GrabberActionsCallbackInterface.OnDrop;
                @Drop.canceled -= m_Wrapper.m_GrabberActionsCallbackInterface.OnDrop;
                @Grab.started -= m_Wrapper.m_GrabberActionsCallbackInterface.OnGrab;
                @Grab.performed -= m_Wrapper.m_GrabberActionsCallbackInterface.OnGrab;
                @Grab.canceled -= m_Wrapper.m_GrabberActionsCallbackInterface.OnGrab;
            }
            m_Wrapper.m_GrabberActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Drop.started += instance.OnDrop;
                @Drop.performed += instance.OnDrop;
                @Drop.canceled += instance.OnDrop;
                @Grab.started += instance.OnGrab;
                @Grab.performed += instance.OnGrab;
                @Grab.canceled += instance.OnGrab;
            }
        }
    }
    public GrabberActions @Grabber => new GrabberActions(this);
    private int m_KeyboardandMouseSchemeIndex = -1;
    public InputControlScheme KeyboardandMouseScheme
    {
        get
        {
            if (m_KeyboardandMouseSchemeIndex == -1) m_KeyboardandMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard and Mouse");
            return asset.controlSchemes[m_KeyboardandMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    public interface IPlayerActions
    {
        void OnTangentAccelerationStart(InputAction.CallbackContext context);
        void OnTangentAccelerationStop(InputAction.CallbackContext context);
        void OnNormalAccelerationStart(InputAction.CallbackContext context);
        void OnNormalAccelerationStop(InputAction.CallbackContext context);
    }
    public interface ICameraActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnMoveStart(InputAction.CallbackContext context);
        void OnMoveStop(InputAction.CallbackContext context);
        void OnInteractionStateChange(InputAction.CallbackContext context);
    }
    public interface ITerraformerActions
    {
        void OnAddStart(InputAction.CallbackContext context);
        void OnAddStop(InputAction.CallbackContext context);
        void OnRemoveStart(InputAction.CallbackContext context);
        void OnRemoveStop(InputAction.CallbackContext context);
    }
    public interface IGrabberActions
    {
        void OnDrop(InputAction.CallbackContext context);
        void OnGrab(InputAction.CallbackContext context);
    }
}
