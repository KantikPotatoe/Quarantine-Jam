// GENERATED AUTOMATICALLY FROM 'Assets/PlayerInputActions.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using Object = UnityEngine.Object;

public class PlayerInputActions : IInputActionCollection, IDisposable
{
    // PlayerControls
    private readonly InputActionMap m_PlayerControls;
    private readonly InputAction m_PlayerControls_Crawl;
    private readonly InputAction m_PlayerControls_Interact;
    private readonly InputAction m_PlayerControls_Move;
    private readonly InputAction m_PlayerControls_Sprint;
    private IPlayerControlsActions m_PlayerControlsActionsCallbackInterface;

    public PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""PlayerControls"",
            ""id"": ""623ff5c0-51e0-4713-8e20-66dcf9054709"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6052d665-c2dc-4fe3-8466-2cccd4981bf2"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""5976e86c-8ed5-42bc-9a70-850de463d5b1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Crawl"",
                    ""type"": ""Button"",
                    ""id"": ""af3f529b-ba9c-41e6-9362-03fe3426d55b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""facdcf10-6c5c-49ce-aa52-5d86379eb853"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""ZQSD"",
                    ""id"": ""d49b7871-97cf-47dc-953f-8847893de218"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""71862661-8756-4398-8e01-6c798971f0f5"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""e3d7366a-e6dc-4336-ba5e-d644c715edca"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""58d5bfbf-c833-484c-8018-ff1a77139776"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""48e30bb6-8362-4f95-9bbe-eb1fd972f724"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c95dea93-fbed-4bd8-9b72-456343cb8ffe"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1784c5d-41b5-494d-9f6d-b88f475ca179"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Crawl"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""38623b1d-528a-43d8-85f3-7e231736bb9d"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // PlayerControls
        m_PlayerControls = asset.FindActionMap("PlayerControls", true);
        m_PlayerControls_Move = m_PlayerControls.FindAction("Move", true);
        m_PlayerControls_Sprint = m_PlayerControls.FindAction("Sprint", true);
        m_PlayerControls_Crawl = m_PlayerControls.FindAction("Crawl", true);
        m_PlayerControls_Interact = m_PlayerControls.FindAction("Interact", true);
    }

    public InputActionAsset asset { get; }
    public PlayerControlsActions PlayerControls => new PlayerControlsActions(this);

    public void Dispose()
    {
        Object.Destroy(asset);
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

    public struct PlayerControlsActions
    {
        private readonly PlayerInputActions m_Wrapper;

        public PlayerControlsActions(PlayerInputActions wrapper)
        {
            m_Wrapper = wrapper;
        }

        public InputAction Move => m_Wrapper.m_PlayerControls_Move;
        public InputAction Sprint => m_Wrapper.m_PlayerControls_Sprint;
        public InputAction Crawl => m_Wrapper.m_PlayerControls_Crawl;
        public InputAction Interact => m_Wrapper.m_PlayerControls_Interact;

        public InputActionMap Get()
        {
            return m_Wrapper.m_PlayerControls;
        }

        public void Enable()
        {
            Get().Enable();
        }

        public void Disable()
        {
            Get().Disable();
        }

        public bool enabled => Get().enabled;

        public static implicit operator InputActionMap(PlayerControlsActions set)
        {
            return set.Get();
        }

        public void SetCallbacks(IPlayerControlsActions instance)
        {
            if (m_Wrapper.m_PlayerControlsActionsCallbackInterface != null)
            {
                Move.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                Move.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                Move.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnMove;
                Sprint.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSprint;
                Sprint.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSprint;
                Sprint.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnSprint;
                Crawl.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCrawl;
                Crawl.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCrawl;
                Crawl.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnCrawl;
                Interact.started -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                Interact.performed -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
                Interact.canceled -= m_Wrapper.m_PlayerControlsActionsCallbackInterface.OnInteract;
            }

            m_Wrapper.m_PlayerControlsActionsCallbackInterface = instance;
            if (instance != null)
            {
                Move.started += instance.OnMove;
                Move.performed += instance.OnMove;
                Move.canceled += instance.OnMove;
                Sprint.started += instance.OnSprint;
                Sprint.performed += instance.OnSprint;
                Sprint.canceled += instance.OnSprint;
                Crawl.started += instance.OnCrawl;
                Crawl.performed += instance.OnCrawl;
                Crawl.canceled += instance.OnCrawl;
                Interact.started += instance.OnInteract;
                Interact.performed += instance.OnInteract;
                Interact.canceled += instance.OnInteract;
            }
        }
    }

    public interface IPlayerControlsActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnCrawl(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
    }
}