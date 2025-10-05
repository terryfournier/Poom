using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FMOD;
using FMOD.Studio;

public enum Colour
{
    AQUA,
    BLACK,
    BLUE,
    BROWN,
    CYAN,
    DARKBLUE,
    FUCHSIA,
    GREEN,
    GREY,
    LIGHTBLUE,
    LIME,
    MAGENTA,
    MAROON,
    NAVY,
    OLIVE,
    ORANGE,
    PURPLE,
    RED,
    SILVER,
    TEAL,
    WHITE,
    YELLOW,

    COLOUR_NB
}

public enum UIColourType
{
    DISABLED,
    HIGHLIGHTED,
    NORMAL,
    PRESSED,
    SELECTED,
    ALL
}

public enum TextEffect
{
    BOLD,
    ITALIC,
    UNDERLINED,
    STRIKETHROUGH,
    SIZED,
    ALL,

    EFFECT_NB
}

public class Helper
{
    #region Constants
    public const uint UNITY_TEXT_SIZE = 12u;
    #endregion

    #region Properties
    public static Keyboard CurrentKeyboard
    {
        get => (KeyboardConnected) ? Keyboard.current : null;
    }
    public static Mouse CurrentMouse
    {
        get => (MouseConnected) ? Mouse.current : null;
    }
    public static Gamepad CurrentGamepad
    {
        get => (GamepadConnected) ? Gamepad.current : null;
    }
    public static int DeviceCount
    {
        get => InputSystem.devices.Count;
    }
    public static int GameDeviceCount
    {
        get => InputSystem.devices.Count(d => d is Keyboard || d is Mouse || d is Gamepad);
    }
    public static bool KeyboardConnected
    {
        get => (InputSystem.devices.Count(d => d is Keyboard) > 0);
    }
    public static bool MouseConnected
    {
        get => (InputSystem.devices.Count(d => d is Mouse) > 0);
    }
    public static bool GamepadConnected
    {
        get => (Gamepad.all.Count > 0);
    }
    public static bool KeyboardUsable
    {
        get => (KeyboardConnected && ControlsManager.KeyboardReady);
    }
    public static bool MouseUsable
    {
        get => (MouseConnected && ControlsManager.MouseReady);
    }
    public static bool GamepadUsable
    {
        get => (GamepadConnected && ControlsManager.GamepadReady);
    }
    public static bool NoDeviceConnected
    {
        get => (InputSystem.devices.Count <= 0);
    }
    public static bool NoGameDeviceConnected
    {
        get => (GameDeviceCount <= 0);
    }
    public static bool KeyboardPress
    {
        get =>
            (KeyboardUsable)
            ? CurrentKeyboard.anyKey.IsPressed()
            : false;
    }
    public static bool MousePress
    {
        get =>
            (MouseUsable)
            ? (CurrentMouse.allControls.Count(x => x is ButtonControl && x.IsPressed()) > 0)
            : false;
    }
    public static bool GamepadPress
    {
        get =>
            (GamepadUsable)
            ? (CurrentGamepad.allControls.Count(x => x is ButtonControl && x.IsPressed()) > 0)
            : false;
    }
    public static bool GameDevicePress
    {
        get => (KeyboardPress || MousePress || GamepadPress);
    }
    #endregion

    #region Methods
    #region Epsilon test
    #region Int
    /// <summary>
    /// Detects if _value is in :
    /// <br><b>[</b><i>-_value</i> ; <i>_value</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-_value</i> ; <i>_value</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestIn(in int _value, in int _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return (_value >= -_epsilon && _value <= _epsilon);
        }

        return (_value > -_epsilon && _value < _epsilon);
    }

    /// <summary>
    /// Detects if _value is in-between :
    /// <br><b>[</b><i>-infinity</i> ; <i>-_value</i><b>]</b> and <b>[</b><i>_value</i> ; <i>infinity</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-infinity</i> ; <i>-_value</i><b>[</b> and <b>]</b><i>_value</i> ; <i>infinity</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestOut(in int _value, in int _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return (_value <= -_epsilon || _value >= _epsilon);
        }

        return (_value < -_epsilon || _value > _epsilon);
    }
    #endregion

    #region Float
    /// <summary>
    /// Detects if _value is in :
    /// <br><b>[</b><i>-_value</i> ; <i>_value</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-_value</i> ; <i>_value</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestIn(in float _value, in float _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return (_value >= -_epsilon && _value <= _epsilon);
        }

        return (_value > -_epsilon && _value < _epsilon);
    }

    /// <summary>
    /// Detects if _value is in-between :
    /// <br><b>[</b><i>-infinity</i> ; <i>-_value</i><b>]</b> and <b>[</b><i>_value</i> ; <i>infinity</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-infinity</i> ; <i>-_value</i><b>[</b> and <b>]</b><i>_value</i> ; <i>infinity</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestOut(in float _value, in float _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return (_value <= -_epsilon || _value >= _epsilon);
        }

        return (_value < -_epsilon || _value > _epsilon);
    }
    #endregion

    #region Vector2
    /// <summary>
    /// Detects if _value is in :
    /// <br><b>[</b><i>-_value</i> ; <i>_value</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-_value</i> ; <i>_value</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestIn(in Vector2 _value, in Vector2 _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.x >= -_epsilon.x && _value.x <= _epsilon.x) &&
                (_value.y >= -_epsilon.y && _value.y <= _epsilon.y);
        }

        return
                (_value.x > -_epsilon.x && _value.x < _epsilon.x) &&
                (_value.y > -_epsilon.y && _value.y < _epsilon.y);
    }

    /// <summary>
    /// Detects if _value is in-between :
    /// <br><b>[</b><i>-infinity</i> ; <i>-_value</i><b>]</b> and <b>[</b><i>_value</i> ; <i>infinity</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-infinity</i> ; <i>-_value</i><b>[</b> and <b>]</b><i>_value</i> ; <i>infinity</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestOut(in Vector2 _value, in Vector2 _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.x <= -_epsilon.x && _value.x >= _epsilon.x) &&
                (_value.y <= -_epsilon.y && _value.y >= _epsilon.y);
        }

        return
                (_value.x < -_epsilon.x && _value.x > _epsilon.x) &&
                (_value.y < -_epsilon.y && _value.y > _epsilon.y);
    }
    #endregion

    #region Vector3
    /// <summary>
    /// Detects if _value is in :
    /// <br><b>[</b><i>-_value</i> ; <i>_value</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-_value</i> ; <i>_value</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestIn(in Vector3 _value, in Vector3 _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.x >= -_epsilon.x && _value.x <= _epsilon.x) &&
                (_value.y >= -_epsilon.y && _value.y <= _epsilon.y) &&
                (_value.z >= -_epsilon.z && _value.z <= _epsilon.z);
        }

        return
                (_value.x > -_epsilon.x && _value.x < _epsilon.x) &&
                (_value.y > -_epsilon.y && _value.y < _epsilon.y) &&
                (_value.z > -_epsilon.z && _value.z < _epsilon.z);
    }

    /// <summary>
    /// Detects if _value is in-between :
    /// <br><b>[</b><i>-infinity</i> ; <i>-_value</i><b>]</b> and <b>[</b><i>_value</i> ; <i>infinity</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-infinity</i> ; <i>-_value</i><b>[</b> and <b>]</b><i>_value</i> ; <i>infinity</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestOut(in Vector3 _value, in Vector3 _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.x <= -_epsilon.x && _value.x >= _epsilon.x) &&
                (_value.y <= -_epsilon.y && _value.y >= _epsilon.y) &&
                (_value.z <= -_epsilon.z && _value.z >= _epsilon.z);
        }

        return
                (_value.x < -_epsilon.x && _value.x > _epsilon.x) &&
                (_value.y < -_epsilon.y && _value.y > _epsilon.y) &&
                (_value.z < -_epsilon.z && _value.z > _epsilon.z);
    }
    #endregion

    #region Vector4
    /// <summary>
    /// Detects if _value is in :
    /// <br><b>[</b><i>-_value</i> ; <i>_value</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-_value</i> ; <i>_value</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestIn(in Vector4 _value, in Vector4 _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.x >= -_epsilon.x && _value.x <= _epsilon.x) &&
                (_value.y >= -_epsilon.y && _value.y <= _epsilon.y) &&
                (_value.z >= -_epsilon.z && _value.z <= _epsilon.z) &&
                (_value.w >= -_epsilon.w && _value.w <= _epsilon.w);
        }

        return
                (_value.x > -_epsilon.x && _value.x < _epsilon.x) &&
                (_value.y > -_epsilon.y && _value.y < _epsilon.y) &&
                (_value.z > -_epsilon.z && _value.z < _epsilon.z) &&
                (_value.w > -_epsilon.w && _value.w < _epsilon.w);
    }

    /// <summary>
    /// Detects if _value is in-between :
    /// <br><b>[</b><i>-infinity</i> ; <i>-_value</i><b>]</b> and <b>[</b><i>_value</i> ; <i>infinity</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-infinity</i> ; <i>-_value</i><b>[</b> and <b>]</b><i>_value</i> ; <i>infinity</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestOut(in Vector4 _value, in Vector4 _epsilon, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.x <= -_epsilon.x && _value.x >= _epsilon.x) &&
                (_value.y <= -_epsilon.y && _value.y >= _epsilon.y) &&
                (_value.z <= -_epsilon.z && _value.z >= _epsilon.z) &&
                (_value.w <= -_epsilon.w && _value.w >= _epsilon.w);
        }

        return
                (_value.x < -_epsilon.x && _value.x > _epsilon.x) &&
                (_value.y < -_epsilon.y && _value.y > _epsilon.y) &&
                (_value.z < -_epsilon.z && _value.z > _epsilon.z) &&
                (_value.w < -_epsilon.w && _value.w > _epsilon.w);
    }
    #endregion

    #region Color
    /// <summary>
    /// Detects if _value is in :
    /// <br><b>[</b><i>-_value</i> ; <i>_value</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-_value</i> ; <i>_value</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestIn(in Color _value, in Color _epsilon, in bool _testAlpha = true, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.r >= -_epsilon.r && _value.r <= _epsilon.r) &&
                (_value.g >= -_epsilon.g && _value.g <= _epsilon.g) &&
                (_value.b >= -_epsilon.b && _value.b <= _epsilon.b) &&
                (_testAlpha)
                ? (_value.a >= -_epsilon.a && _value.a <= _epsilon.a)
                : true;
        }

        return
                (_value.r > -_epsilon.r && _value.r < _epsilon.r) &&
                (_value.g > -_epsilon.g && _value.g < _epsilon.g) &&
                (_value.b > -_epsilon.b && _value.b < _epsilon.b) &&
                (_testAlpha)
                ? (_value.a > -_epsilon.a && _value.a < _epsilon.a)
                : true;
    }

    /// <summary>
    /// Detects if _value is in-between :
    /// <br><b>[</b><i>-infinity</i> ; <i>-_value</i><b>]</b> and <b>[</b><i>_value</i> ; <i>infinity</i><b>]</b> when <i>_inclusive</i> is <b>true</b></br>
    /// <br><b>]</b><i>-infinity</i> ; <i>-_value</i><b>[</b> and <b>]</b><i>_value</i> ; <i>infinity</i><b>[</b> when <i>_inclusive</i> is <b>false</b></br>
    /// </summary>
    /// <param name="_value">The value to test</param>
    /// <param name="_epsilon">The min and max values to compare with _value</param>
    /// <param name="_inclusive">Allow/Disallow equality between <b>_value</b> and <b>_epsilon</b></param>
    /// <returns></returns>
    public static bool EpsilonTestOut(in Color _value, in Color _epsilon, in bool _testAlpha = true, in bool _inclusive = true)
    {
        if (_inclusive)
        {
            return
                (_value.r <= -_epsilon.r && _value.r >= _epsilon.r) &&
                (_value.g <= -_epsilon.g && _value.g >= _epsilon.g) &&
                (_value.b <= -_epsilon.b && _value.b >= _epsilon.b) &&
                (_testAlpha)
                ? (_value.a <= -_epsilon.a && _value.a >= _epsilon.a)
                : true;
        }

        return
                (_value.r < -_epsilon.r && _value.r > _epsilon.r) &&
                (_value.g < -_epsilon.g && _value.g > _epsilon.g) &&
                (_value.b < -_epsilon.b && _value.b > _epsilon.b) &&
                (_testAlpha)
                ? (_value.a < -_epsilon.a && _value.a > _epsilon.a)
                : true;
    }
    #endregion
    #endregion

    #region Print in colour
    #region Log
    /// <summary>
    /// UnityEngine.Debug.Log _text in _colour (use Colour enum)
    /// </summary>
    /// <param name="_colour">The colour you want _message to be</param>
    /// <param name="_message">The message you want to write to the console</param>
    /// <returns></returns>
    public static void LogColour(in Colour _colour, in object _message)
    {
        UnityEngine.Debug.Log("<color=" + _colour.ToString() + ">" + _message + "</color>");
    }
    /// <summary>
    /// UnityEngine.Debug.Log _text in _colour (use hexadecimal value)
    /// </summary>
    /// <param name="_hexColour">The colour you want _message to be</param>
    /// <param name="_message">The message you want to write to the console</param>
    /// <returns></returns>
    public static void LogColour(in string _hexColour, in object _message)
    {
        UnityEngine.Debug.Log("<color=" + _hexColour + ">" + _message + "</color>");
    }
    #endregion

    #region Warning
    /// <summary>
    /// UnityEngine.Debug.LogWarning _text in _colour (use Colour enum)
    /// </summary>
    /// <param name="_colour">The colour you want _message to be</param>
    /// <param name="_message">The message you want to write to the console</param>
    /// <returns></returns>
    public static void WarningColour(in Colour _colour, in object _message)
    {
        UnityEngine.Debug.LogWarning("<color=" + _colour.ToString() + ">" + _message + "</color>");
    }
    /// <summary>
    /// UnityEngine.Debug.Log _text in _colour (use hexadecimal value)
    /// </summary>
    /// <param name="_hexColour">The colour you want _message to be</param>
    /// <param name="_message">The message you want to write to the console</param>
    /// <returns></returns>
    public static void WarningColour(in string _hexColour, in object _message)
    {
        UnityEngine.Debug.LogWarning("<color=" + _hexColour + ">" + _message + "</color>");
    }
    #endregion

    #region Error
    /// <summary>
    /// UnityEngine.Debug.LogError _text in _colour (use Colour enum)
    /// </summary>
    /// <param name="_colour">The colour you want _message to be</param>
    /// <param name="_message">The message you want to write to the console</param>
    /// <returns></returns>
    public static void ErrorColour(in Colour _colour, in object _message)
    {
        UnityEngine.Debug.LogError("<color=" + _colour.ToString() + ">" + _message + "</color>");
    }
    /// <summary>
    /// UnityEngine.Debug.Log _text in _colour (use hexadecimal value)
    /// </summary>
    /// <param name="_hexColour">The colour you want _message to be</param>
    /// <param name="_message">The message you want to write to the console</param>
    /// <returns></returns>
    public static void ErrorColour(in string _hexColour, in object _message)
    {
        UnityEngine.Debug.LogError("<color=" + _hexColour + ">" + _message + "</color>");
    }
    #endregion
    #endregion

    #region Print with effect
    #region Log
    public static void LogEffect(in TextEffect _effect, in object _message, in uint _size = 12u)
    {
        if (_effect != TextEffect.ALL)
        {
            if (_effect != TextEffect.SIZED)
            {
                char effectChar = _effect.ToString().ToLower()[0];

                UnityEngine.Debug.Log("<" + effectChar + ">" + _message + "</" + effectChar + ">");

                return;
            }

            LogSized(_size, _message);

            return;
        }

        UnityEngine.Debug.Log("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    public static void LogBold(in object _message)
    {
        UnityEngine.Debug.Log("<b>" + _message + "</b>");
    }
    public static void LogItalic(in object _message)
    {
        UnityEngine.Debug.Log("<i>" + _message + "</i>");
    }
    public static void LogUnderlined(in object _message)
    {
        UnityEngine.Debug.Log("<u>" + _message + "</u>");
    }
    public static void LogStrikethrough(in object _message)
    {
        UnityEngine.Debug.Log("<s>" + _message + "</s>");
    }
    public static void LogSized(in uint _size, in object _message)
    {
        UnityEngine.Debug.Log("<size=" + _size + ">" + _message + "</size>");
    }
    public static void LogEffects(in object _message, in uint _size = 12u, params TextEffect[] _effects)
    {
        string[] effects = new string[(int)(TextEffect.EFFECT_NB) - 1];
        int differentEffectCount = 1;
        char effectChar = (char)(0);
        bool canSkip = false;

        for (int i = 0; i < effects.Length; i++)
        {
            effects[i] = " ";
        }

        for (int i = 0; i < _effects.Length; i++)
        {
            if (_effects[i] == TextEffect.ALL)
            {
                canSkip = true;
                break;
            }

            if (_effects[i] != TextEffect.EFFECT_NB)
            {
                if (i == 0)
                {
                    effectChar = _effects[i].ToString().ToLower()[0];
                    effects[i] = effectChar.ToString();
                }
                else if (_effects[i] != _effects[i - 1])
                {
                    if (_effects[i] != TextEffect.SIZED)
                    {
                        effectChar = _effects[i].ToString().ToLower()[0];
                        effects[i] = effectChar.ToString();
                    }
                    else
                    {
                        effects[i] = TextEffect.SIZED.ToString().ToLower();
                    }

                    differentEffectCount++;
                }

                if (differentEffectCount == 5)
                {
                    break;
                }
            }
        }

        if (!canSkip)
        {
            List<string> effectList = new List<string>();

            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] != " ")
                {
                    effectList.Add(effects[i]);
                }
            }

            string fxBeg = "";
            string fxEnd = "";

            for (int i = 0; i < effectList.Count; i++)
            {
                if (effectList[i] != "sized")
                {
                    fxBeg += "<" + effectList[i] + ">";
                }
                else
                {
                    fxBeg += "<size=" + _size + ">";
                }

                fxEnd += "</" + effectList[i] + ">";
            }

            UnityEngine.Debug.Log(fxBeg + _message + fxEnd);

            return;
        }

        UnityEngine.Debug.Log("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    public static void LogEffects(in object _message, in TextEffect[] _effects, in uint _size = 12u)
    {
        bool canSkip = false;
        TextEffect curr = 0;
        string fxBeg = "";
        string fxEnd = "";

        for (int i = 0; i < _effects.Length; i++)
        {
            curr = _effects[i];

            if (curr == TextEffect.ALL)
            {
                canSkip = true;
                break;
            }

            if (curr != TextEffect.EFFECT_NB)
            {
                if (curr == TextEffect.SIZED)
                {
                    fxBeg += "<size=" + _size + ">";
                    fxEnd += "</size>";
                }
                else
                {
                    string effectCharStr = curr.ToString().ToLower()[0].ToString();

                    fxBeg += "<" + effectCharStr + ">";
                    fxEnd += "</" + effectCharStr + ">";
                }
            }
        }

        if (!canSkip)
        {
            UnityEngine.Debug.Log(fxBeg + _message + fxEnd);
            return;
        }

        UnityEngine.Debug.Log("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    #endregion

    #region Warning
    public static void WarningEffect(in TextEffect _effect, in object _message, in uint _size = 12u)
    {
        if (_effect != TextEffect.ALL)
        {
            if (_effect != TextEffect.SIZED)
            {
                char effectChar = _effect.ToString().ToLower()[0];

                UnityEngine.Debug.LogWarning("<" + effectChar + ">" + _message + "</" + effectChar + ">");

                return;
            }

            WarningSized(_size, _message);

            return;
        }

        UnityEngine.Debug.LogWarning("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    public static void WarningBold(in object _message)
    {
        UnityEngine.Debug.LogWarning("<b>" + _message + "</b>");
    }
    public static void WarningItalic(in object _message)
    {
        UnityEngine.Debug.LogWarning("<i>" + _message + "</i>");
    }
    public static void WarningUnderlined(in object _message)
    {
        UnityEngine.Debug.LogWarning("<u>" + _message + "</u>");
    }
    public static void WarningStrikethrough(in object _message)
    {
        UnityEngine.Debug.LogWarning("<s>" + _message + "</s>");
    }
    public static void WarningSized(in uint _size, in object _message)
    {
        UnityEngine.Debug.LogWarning("<size=" + _size + ">" + _message + "</size>");
    }
    public static void WarningEffects(in object _message, in uint _size = 12u, params TextEffect[] _effects)
    {
        string[] effects = new string[(int)(TextEffect.EFFECT_NB) - 1];
        int differentEffectCount = 1;
        char effectChar = (char)(0);
        bool canSkip = false;

        for (int i = 0; i < effects.Length; i++)
        {
            effects[i] = " ";
        }

        for (int i = 0; i < _effects.Length; i++)
        {
            if (_effects[i] == TextEffect.ALL)
            {
                canSkip = true;
                break;
            }

            if (_effects[i] != TextEffect.EFFECT_NB)
            {
                if (i == 0)
                {
                    effectChar = _effects[i].ToString().ToLower()[0];
                    effects[i] = effectChar.ToString();
                }
                else if (_effects[i] != _effects[i - 1])
                {
                    if (_effects[i] != TextEffect.SIZED)
                    {
                        effectChar = _effects[i].ToString().ToLower()[0];
                        effects[i] = effectChar.ToString();
                    }
                    else
                    {
                        effects[i] = TextEffect.SIZED.ToString().ToLower();
                    }

                    differentEffectCount++;
                }

                if (differentEffectCount == 5)
                {
                    break;
                }
            }
        }

        if (!canSkip)
        {
            List<string> effectList = new List<string>();

            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] != " ")
                {
                    effectList.Add(effects[i]);
                }
            }

            string fxBeg = "";
            string fxEnd = "";

            for (int i = 0; i < effectList.Count; i++)
            {
                if (effectList[i] != "sized")
                {
                    fxBeg += "<" + effectList[i] + ">";
                }
                else
                {
                    fxBeg += "<size=" + _size + ">";
                }

                fxEnd += "</" + effectList[i] + ">";
            }

            UnityEngine.Debug.LogWarning(fxBeg + _message + fxEnd);

            return;
        }

        UnityEngine.Debug.LogWarning("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    public static void WarningEffects(in object _message, in TextEffect[] _effects, in uint _size = 12u)
    {
        bool canSkip = false;
        TextEffect curr = 0;
        string fxBeg = "";
        string fxEnd = "";

        for (int i = 0; i < _effects.Length; i++)
        {
            curr = _effects[i];

            if (curr == TextEffect.ALL)
            {
                canSkip = true;
                break;
            }

            if (curr != TextEffect.EFFECT_NB)
            {
                if (curr == TextEffect.SIZED)
                {
                    fxBeg += "<size=" + _size + ">";
                    fxEnd += "</size>";
                }
                else
                {
                    string effectCharStr = curr.ToString().ToLower()[0].ToString();

                    fxBeg += "<" + effectCharStr + ">";
                    fxEnd += "</" + effectCharStr + ">";
                }
            }
        }

        if (!canSkip)
        {
            UnityEngine.Debug.LogWarning(fxBeg + _message + fxEnd);
            return;
        }

        UnityEngine.Debug.LogWarning("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    #endregion

    #region Error
    public static void ErrorEffect(in TextEffect _effect, in object _message, in uint _size = 12u)
    {
        if (_effect != TextEffect.ALL)
        {
            if (_effect != TextEffect.SIZED)
            {
                char effectChar = _effect.ToString().ToLower()[0];

                UnityEngine.Debug.LogError("<" + effectChar + ">" + _message + "</" + effectChar + ">");

                return;
            }

            ErrorSized(_size, _message);

            return;
        }

        UnityEngine.Debug.LogError("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    public static void ErrorBold(in object _message)
    {
        UnityEngine.Debug.LogError("<b>" + _message + "</b>");
    }
    public static void ErrorItalic(in object _message)
    {
        UnityEngine.Debug.LogError("<i>" + _message + "</i>");
    }
    public static void ErrorUnderlined(in object _message)
    {
        UnityEngine.Debug.LogError("<u>" + _message + "</u>");
    }
    public static void ErrorStrikethrough(in object _message)
    {
        UnityEngine.Debug.LogError("<s>" + _message + "</s>");
    }
    public static void ErrorSized(in uint _size, in object _message)
    {
        UnityEngine.Debug.LogError("<size=" + _size + ">" + _message + "</size>");
    }
    public static void ErrorEffects(in object _message, in uint _size = 12u, params TextEffect[] _effects)
    {
        string[] effects = new string[(int)(TextEffect.EFFECT_NB) - 1];
        int differentEffectCount = 1;
        char effectChar = (char)(0);
        bool canSkip = false;

        for (int i = 0; i < effects.Length; i++)
        {
            effects[i] = " ";
        }

        for (int i = 0; i < _effects.Length; i++)
        {
            if (_effects[i] == TextEffect.ALL)
            {
                canSkip = true;
                break;
            }

            if (_effects[i] != TextEffect.EFFECT_NB)
            {
                if (i == 0)
                {
                    effectChar = _effects[i].ToString().ToLower()[0];
                    effects[i] = effectChar.ToString();
                }
                else if (_effects[i] != _effects[i - 1])
                {
                    if (_effects[i] != TextEffect.SIZED)
                    {
                        effectChar = _effects[i].ToString().ToLower()[0];
                        effects[i] = effectChar.ToString();
                    }
                    else
                    {
                        effects[i] = TextEffect.SIZED.ToString().ToLower();
                    }

                    differentEffectCount++;
                }

                if (differentEffectCount == 5)
                {
                    break;
                }
            }
        }

        if (!canSkip)
        {
            List<string> effectList = new List<string>();

            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i] != " ")
                {
                    effectList.Add(effects[i]);
                }
            }

            string fxBeg = "";
            string fxEnd = "";

            for (int i = 0; i < effectList.Count; i++)
            {
                if (effectList[i] != "sized")
                {
                    fxBeg += "<" + effectList[i] + ">";
                }
                else
                {
                    fxBeg += "<size=" + _size + ">";
                }

                fxEnd += "</" + effectList[i] + ">";
            }

            UnityEngine.Debug.LogError(fxBeg + _message + fxEnd);

            return;
        }

        UnityEngine.Debug.LogError("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    public static void ErrorEffects(in object _message, in TextEffect[] _effects, in uint _size = 12u)
    {
        bool canSkip = false;
        TextEffect curr = 0;
        string fxBeg = "";
        string fxEnd = "";

        for (int i = 0; i < _effects.Length; i++)
        {
            curr = _effects[i];

            if (curr == TextEffect.ALL)
            {
                canSkip = true;
                break;
            }

            if (curr != TextEffect.EFFECT_NB)
            {
                if (curr == TextEffect.SIZED)
                {
                    fxBeg += "<size=" + _size + ">";
                    fxEnd += "</size>";
                }
                else
                {
                    string effectCharStr = curr.ToString().ToLower()[0].ToString();

                    fxBeg += "<" + effectCharStr + ">";
                    fxEnd += "</" + effectCharStr + ">";
                }
            }
        }

        if (!canSkip)
        {
            UnityEngine.Debug.LogError(fxBeg + _message + fxEnd);
            return;
        }

        UnityEngine.Debug.LogError("<b><i><u><s><size=" + _size + ">" + _message + "</size></s></u></i></b>");
    }
    #endregion
    #endregion

    #region Print in colour with condition
    #region Log
    /// <summary>
    /// Writes <i>_message</i> in <i>_colour1</i> when <i>_value</i> is equal to <b>_conditionValue</b>, writes it in <i>_colour2</i> <b>otherwise</b>
    /// </summary>
    /// <param name="_message">The message you want to display in the console</param>
    /// <param name="_value">The value you want to test (true/false)</param>
    /// <param name="_conditionValue"></param>
    /// <param name="_colour1">The colour of _message when _value is true</param>
    /// <param name="_colour2">The colour of _message when _value is false</param>
    public static void LogCondition(in object _message, in object _value, in object _conditionValue, in Colour _colour1 = Colour.GREEN, in Colour _colour2 = Colour.RED)
    {
        UnityEngine.Debug.Log("<color=" + ((_value == _conditionValue) ? _colour1.ToString() : _colour2.ToString()) + ">" + _message + "</color>");
    }

    /// <summary>
    /// Override for booleans
    /// <br>Writes <i>_message</i> in <i>_colour1</i> when <i>_condition</i> is <b>true</b>, writes it in <i>_colour2</i> <b>otherwise</b></br>
    /// </summary>
    /// <param name="_message">The message you want to display in the console</param>
    /// <param name="_condition">The value you want to test (true/false)</param>
    /// <param name="_colour1">The colour of _message when _condition is true</param>
    /// <param name="_colour2">The colour of _message when _condition is false</param>
    public static void LogCondition(in object _message, in bool _condition, in Colour _colour1 = Colour.GREEN, in Colour _colour2 = Colour.RED)
    {
        UnityEngine.Debug.Log("<color=" + ((_condition) ? _colour1.ToString() : _colour2.ToString()) + ">" + _message + "</color>");
    }
    #endregion

    #region Warning
    /// <summary>
    /// Writes <i>_message</i> in <i>_colour1</i> when <i>_value</i> is equal to <b>_conditionValue</b>, writes it in <i>_colour2</i> <b>otherwise</b>
    /// </summary>
    /// <param name="_message">The message you want to display in the console</param>
    /// <param name="_value">The value you want to test (true/false)</param>
    /// <param name="_conditionValue"></param>
    /// <param name="_colour1">The colour of _message when _value is true</param>
    /// <param name="_colour2">The colour of _message when _value is false</param>
    public static void WarningCondition(in object _message, in object _value, in object _conditionValue, in Colour _colour1 = Colour.GREEN, in Colour _colour2 = Colour.RED)
    {
        UnityEngine.Debug.LogWarning("<color=" + ((_value == _conditionValue) ? _colour1.ToString() : _colour2.ToString()) + ">" + _message + "</color>");
    }

    /// <summary>
    /// Override for booleans
    /// <br>Writes <i>_message</i> in <i>_colour1</i> when <i>_condition</i> is <b>true</b>, writes it in <i>_colour2</i> <b>otherwise</b></br>
    /// </summary>
    /// <param name="_message">The message you want to display in the console</param>
    /// <param name="_condition">The value you want to test (true/false)</param>
    /// <param name="_colour1">The colour of _message when _condition is true</param>
    /// <param name="_colour2">The colour of _message when _condition is false</param>
    public static void WarningCondition(in object _message, in bool _condition, in Colour _colour1 = Colour.GREEN, in Colour _colour2 = Colour.RED)
    {
        UnityEngine.Debug.LogWarning("<color=" + ((_condition) ? _colour1.ToString() : _colour2.ToString()) + ">" + _message + "</color>");
    }
    #endregion

    #region Error
    /// <summary>
    /// Writes <i>_message</i> in <i>_colour1</i> when <i>_value</i> is equal to <b>_conditionValue</b>, writes it in <i>_colour2</i> <b>otherwise</b>
    /// </summary>
    /// <param name="_message">The message you want to display in the console</param>
    /// <param name="_value">The value you want to test (true/false)</param>
    /// <param name="_conditionValue"></param>
    /// <param name="_colour1">The colour of _message when _value is true</param>
    /// <param name="_colour2">The colour of _message when _value is false</param>
    public static void ErrorCondition(in object _message, in object _value, in object _conditionValue, in Colour _colour1 = Colour.GREEN, in Colour _colour2 = Colour.RED)
    {
        UnityEngine.Debug.LogError("<color=" + ((_value == _conditionValue) ? _colour1.ToString() : _colour2.ToString()) + ">" + _message + "</color>");
    }

    /// <summary>
    /// Override for booleans
    /// <br>Writes <i>_message</i> in <i>_colour1</i> when <i>_condition</i> is <b>true</b>, writes it in <i>_colour2</i> <b>otherwise</b></br>
    /// </summary>
    /// <param name="_message">The message you want to display in the console</param>
    /// <param name="_condition">The value you want to test (true/false)</param>
    /// <param name="_colour1">The colour of _message when _condition is true</param>
    /// <param name="_colour2">The colour of _message when _condition is false</param>
    public static void ErrorCondition(in object _message, in bool _condition, in Colour _colour1 = Colour.GREEN, in Colour _colour2 = Colour.RED)
    {
        UnityEngine.Debug.LogError("<color=" + ((_condition) ? _colour1.ToString() : _colour2.ToString()) + ">" + _message + "</color>");
    }
    #endregion
    #endregion

    #region GetNormalOfMesh
    /// <summary>
    /// Returns the normal of the whole mesh in <b>local</b> coordinates
    /// <br>To get its normal in <b>global</b> coordinates use <b>transform.TransformDirection()</b>;</br>
    /// </summary>
    /// <param name="_mesh">The mesh which we want the normal of</param>
    /// <returns>The normal of _mesh in local coordinates</returns>
    public static Vector3 GetNormalOfMesh(in Mesh _mesh)
    {
        if (!_mesh || !_mesh.isReadable)
        {
            ErrorColour(Colour.WHITE, "Helper.GetNormalOfMesh : _mesh is null or not readable");

            return Vector3.zero;
        }

        Vector3 normal = Vector3.zero;

        foreach (Vector3 pointNormal in _mesh.normals)
        {
            normal += pointNormal;
        }

        normal.Normalize();
        normal /= _mesh.normals.Length;

        return normal;
    }
    /// <summary>
    /// Returns the normal of the whole mesh in <b>global</b> coordinates
    /// </summary>
    /// <param name="_mesh">The mesh which we want the normal of</param>
    /// <param name="_transform">The transform of the object of which we want the mesh</param>
    /// <returns>The normal of _mesh in global coordinates</returns>
    public static Vector3 GetWeightedNormalOfMesh(in Mesh _mesh, in Transform _transform)
    {
        // Mesh validity detection
        if (!_mesh || !_mesh.isReadable)
        {
            ErrorColour(Colour.WHITE, "Helper.GetWeightedNormalOfMesh : Mesh is null or not readable");
            return Vector3.zero;
        }

        // Fetching of vertices and triangles
        Vector3[] vertices = _mesh.vertices;
        int[] triangles = _mesh.triangles;
        int triCount = triangles.Length / 3;

        Vector3 weightedNormal = Vector3.zero;

        // Weighted normal calculation
        for (int i = 0; i < triCount; i++)
        {
            int index0 = triangles[i * 3];
            int index1 = triangles[i * 3 + 1];
            int index2 = triangles[i * 3 + 2];

            Vector3 v0 = vertices[index0];
            Vector3 v1 = vertices[index1];
            Vector3 v2 = vertices[index2];

            Vector3 side1 = v1 - v0;
            Vector3 side2 = v2 - v0;

            weightedNormal += Vector3.Cross(side1, side2);
        }

        // Fallback if degenerate mesh
        if (weightedNormal == Vector3.zero) { return Vector3.up; }

        // Normalised weighted normal of _mesh
        weightedNormal.Normalize();
        return _transform.TransformDirection(weightedNormal);
    }

    #endregion

    #region Prefab methods
    private static GameObject FindInstance(in string _prefabName)
    {
        GameObject instance = GameObject.Find(_prefabName);
        if (instance != null) { return instance; }

        instance = GameObject.Find(_prefabName + "(Clone)");
        return instance;
    }

    /// <summary>
    /// Searches in the scene if an instance of a prefab named <b>_prefabName</b> or <b>_prefabName(Clone)</b> exists
    /// </summary>
    /// <param name="_prefabName">The name of the prefab to search the existence of</param>
    /// <returns><b>true</b> if an instance is in the scene
    /// <br><b>false</b> otherwise</br></returns>
    public static bool PrefabExists(in string _prefabName)
    {
        return (FindInstance(_prefabName) != null);
    }

    public static GameObject GetInstance(in string _prefabName)
    {
        string cloneName = _prefabName + "(Clone)";

        if (PrefabExists(_prefabName)) { LogColour(Colour.GREEN, "Found object '" + _prefabName + "'"); return GameObject.Find(_prefabName); }
        if (PrefabExists(cloneName)) { LogColour(Colour.CYAN, "Found clone '" + cloneName + "'"); return GameObject.Find(cloneName); }

        ErrorColour(Colour.WHITE, "GetInstance : Prefab '" + _prefabName + "' has no instance in the scene");

        return null;
    }
    #endregion

    #region GetAxis
    public static float GetAxis_Keyboard(in bool _xAxis)
    {
        bool isMovingForward = ControlsManager.HasPressed_Keyboard(KeyboardActions.MOVE_FWD, true);
        bool isMovingBack = ControlsManager.HasPressed_Keyboard(KeyboardActions.MOVE_BACK, true);
        bool isMovingLeft = ControlsManager.HasPressed_Keyboard(KeyboardActions.MOVE_LEFT, true);
        bool isMovingRight = ControlsManager.HasPressed_Keyboard(KeyboardActions.MOVE_RIGHT, true);
        float axisValue = 0f;

        if (_xAxis)
        {
            if (isMovingLeft || isMovingRight)
            {
                axisValue = (isMovingLeft) ? -1f : 1f;
            }

            return axisValue;
        }

        if (isMovingBack || isMovingForward)
        {
            axisValue = (isMovingBack) ? -1f : 1f;
        }

        return axisValue;
    }
    /// <summary>
    /// Returns a value between <b>-1</b> and <b>1</b> (both included) for the desired axis
    /// <br><b>-1</b> : moving <b>down</b>/<b>left</b> (if <i>_xAxis</i> is <b>false</b>)</br>
    /// <br><b>0</b> : <b>not</b> moving</br>
    /// <br><b>1</b> : moving <b>up</b>/<b>right</b> (if <i>_xAxis</i> is <b>true</b>)</br>
    /// </summary>
    /// <param name="_xAxis">Determines if we use the X axis (horizontal movement) or the Y axis (vertical movement)</param>
    /// <returns></returns>
    public static float GetAxis_Mouse(in bool _xAxis, in bool _forScroll = false)
    {
        if (!MouseUsable)
        {
            ErrorColour(Colour.WHITE, "GetAxis_Mouse: No mouse connected, returns 0f");

            return 0f;
        }

        Mouse current = Mouse.current;
        Vector2 value = Vector2.zero;

        if (_forScroll)
        {
            value = current.scroll.ReadValue();

            return (_xAxis) ? value.x : value.y;
        }
        else
        {
            value = current.delta.ReadValue();

            return (_xAxis) ? value.x : value.y;
        }
    }

    /// <summary>
    /// Returns a value between <b>-1</b> and <b>1</b> (both included) for the desired axis
    /// <br><b>-1</b> : moving <b>down</b>/<b>left</b> (if <i>_xAxis</i> is <b>false</b>)</br>
    /// <br><b>0</b> : <b>not</b> moving</br>
    /// <br><b>1</b> : moving <b>up</b>/<b>right</b> (if <i>_xAxis</i> is <b>true</b>)</br>
    /// </summary>
    /// <param name="_xAxis">Determines if we use the X axis (horizontal movement) or the Y axis (vertical movement)</param>
    /// <param name="_leftStick">Determines if we query the Left joystick or the Right joystick</param>
    /// <returns></returns>
    public static float GetAxis_Gamepad(in bool _xAxis, in bool _leftStick)
    {
        if (!GamepadUsable)
        {
            ErrorColour(Colour.WHITE, "GetAxis_Gamepad: No gamepad connected, returns 0f");

            return 0f;
        }

        Gamepad current = Gamepad.current;

        if (_leftStick)
        {
            StickControl leftStick = current.leftStick;

            return (_xAxis) ? leftStick.x.ReadValue() : leftStick.y.ReadValue();
        }

        StickControl rightStick = current.rightStick;

        return (_xAxis) ? rightStick.x.ReadValue() : rightStick.y.ReadValue();
    }
    #endregion

    #region ReplaceUIColour
    public static void ReplaceUIColour(in Selectable _selectable, in Color _newColour, in UIColourType _colourToChange = UIColourType.NORMAL)
    {
        ColorBlock colourBlock = _selectable.colors;

        switch (_colourToChange)
        {
            case UIColourType.DISABLED: { colourBlock.disabledColor = _newColour; } break;
            case UIColourType.HIGHLIGHTED: { colourBlock.highlightedColor = _newColour; } break;
            case UIColourType.NORMAL: { colourBlock.normalColor = _newColour; } break;
            case UIColourType.PRESSED: { colourBlock.pressedColor = _newColour; } break;
            case UIColourType.SELECTED: { colourBlock.selectedColor = _newColour; } break;
            case UIColourType.ALL:
                {
                    colourBlock.disabledColor = _newColour;
                    colourBlock.highlightedColor = _newColour;
                    colourBlock.normalColor = _newColour;
                    colourBlock.pressedColor = _newColour;
                    colourBlock.selectedColor = _newColour;
                }
                break;
            default: { return; }
        }

        _selectable.colors = colourBlock;
    }
    #endregion

    #region GetFmodEventInstanceGuid
    public static string GetFmodEventInstanceGuid(in EventInstance _eventInstance)
    {
        _eventInstance.getDescription(out EventDescription description);
        description.getID(out GUID guid);

        return guid.ToString();
    }
    #endregion

    #region IsAtScene
    public static bool IsAtScene(in string _sceneName)
    {
        return (string.Equals(SceneManager.GetActiveScene().name, _sceneName));
    }
    public static bool IsAtScene(in int _sceneBuildIndex)
    {
        return (int.Equals(SceneManager.GetActiveScene().buildIndex, _sceneBuildIndex));
    }
    public static bool IsAtScene(in Scene _scene)
    {
        return (Scene.Equals(SceneManager.GetActiveScene(), _scene));
    }
    #endregion

    #region Approximately
    public static bool Approximately(in Vector2 _a, in Vector2 _b)
    {
        return (
            Mathf.Approximately(_a.x, _b.x) &&
            Mathf.Approximately(_a.y, _b.y)
        );
    }
    public static bool Approximately(in Vector3 _a, in Vector3 _b)
    {
        return (
            Mathf.Approximately(_a.x, _b.x) &&
            Mathf.Approximately(_a.y, _b.y) &&
            Mathf.Approximately(_a.z, _b.z)
        );
    }
    public static bool Approximately(in Vector4 _a, in Vector4 _b)
    {
        return (
            Mathf.Approximately(_a.x, _b.x) &&
            Mathf.Approximately(_a.y, _b.y) &&
            Mathf.Approximately(_a.z, _b.z) &&
            Mathf.Approximately(_a.w, _b.w)
        );
    }
    public static bool Approximately(in Color _a, in Color _b, in bool _testAlpha = true)
    {
        if (!_testAlpha)
        {
            return (
                Mathf.Approximately(_a.r, _b.r) &&
                Mathf.Approximately(_a.g, _b.g) &&
                Mathf.Approximately(_a.b, _b.b)
            );
        }

        return (
            Mathf.Approximately(_a.r, _b.r) &&
            Mathf.Approximately(_a.g, _b.g) &&
            Mathf.Approximately(_a.b, _b.b) &&
            Mathf.Approximately(_a.a, _b.a)
        );
    }
    #endregion

    #region Clamp
    public static void Clamp(ref Vector2 _value, in Vector2 _min, in Vector2 _max)
    {
        if (_value.x < _min.x) { _value.x = _min.x; }
        else if (_value.x > _max.x) { _value.x = _max.x; }

        if (_value.y < _min.y) { _value.y = _min.y; }
        else if (_value.y > _max.y) { _value.y = _max.y; }
    }
    public static void Clamp(ref Vector3 _value, in Vector3 _min, in Vector3 _max)
    {
        if (_value.x < _min.x) { _value.x = _min.x; }
        else if (_value.x > _max.x) { _value.x = _max.x; }

        if (_value.y < _min.y) { _value.y = _min.y; }
        else if (_value.y > _max.y) { _value.y = _max.y; }

        if (_value.z < _min.z) { _value.z = _min.z; }
        else if (_value.z > _max.z) { _value.z = _max.z; }
    }
    public static void Clamp(ref Vector4 _value, in Vector4 _min, in Vector4 _max)
    {
        if (_value.x < _min.x) { _value.x = _min.x; }
        else if (_value.x > _max.x) { _value.x = _max.x; }

        if (_value.y < _min.y) { _value.y = _min.y; }
        else if (_value.y > _max.y) { _value.y = _max.y; }

        if (_value.z < _min.z) { _value.z = _min.z; }
        else if (_value.z > _max.z) { _value.z = _max.z; }

        if (_value.w < _min.w) { _value.w = _min.w; }
        else if (_value.w > _max.w) { _value.w = _max.w; }
    }
    public static void Clamp(ref Color _value, in Color _min, in Color _max, in bool _clampAlpha = true)
    {
        if (_value.r < _min.r) { _value.r = _min.r; }
        else if (_value.r > _max.r) { _value.r = _max.r; }

        if (_value.g < _min.g) { _value.g = _min.g; }
        else if (_value.g > _max.g) { _value.g = _max.g; }

        if (_value.b < _min.b) { _value.b = _min.b; }
        else if (_value.b > _max.b) { _value.b = _max.b; }

        if (_clampAlpha)
        {
            if (_value.a < _min.a) { _value.a = _min.a; }
            else if (_value.a > _max.a) { _value.a = _max.a; }
        }
    }
    #endregion

    #region Toggle
    public static void ToggleBool(ref bool _valueToToggle)
    {
        _valueToToggle = !_valueToToggle;
    }
    #endregion

    #region Abs
    public static Vector2 Abs(Vector2 _v2)
    {
        _v2.x = Mathf.Abs(_v2.x);
        _v2.y = Mathf.Abs(_v2.y);

        return _v2;
    }
    public static Vector3 Abs(Vector3 _v3)
    {
        _v3.x = Mathf.Abs(_v3.x);
        _v3.y = Mathf.Abs(_v3.y);
        _v3.z = Mathf.Abs(_v3.z);

        return _v3;
    }
    public static Vector4 Abs(Vector4 _v4)
    {
        _v4.x = Mathf.Abs(_v4.x);
        _v4.y = Mathf.Abs(_v4.y);
        _v4.z = Mathf.Abs(_v4.z);
        _v4.w = Mathf.Abs(_v4.w);

        return _v4;
    }
    public static Color Abs(Color _col, in bool _includeAlpha)
    {
        _col.r = Mathf.Abs(_col.r);
        _col.g = Mathf.Abs(_col.g);
        _col.b = Mathf.Abs(_col.b);

        if (_includeAlpha)
        {
            _col.a = Mathf.Abs(_col.a);
        }

        return _col;
    }
    #endregion

    #region FetchManagers
    //public static void FetchManager(out object _manager)
    public static void FetchUtilityManager(out UtilityManagers _utilityManager)
    {
        _utilityManager = GameObject.FindAnyObjectByType<UtilityManagers>();
    }
    public static void FetchControlsManager(out ControlsManager _controlsManager)
    {
        _controlsManager = GameObject.FindAnyObjectByType<ControlsManager>();
    }
    public static void FetchMouseManager(out MouseManager _mouseManager)
    {
        _mouseManager = GameObject.FindAnyObjectByType<MouseManager>();
    }
    public static void FetchAudioManager(out AudioManager _audioManager)
    {
        _audioManager = GameObject.FindAnyObjectByType<AudioManager>();
    }
    public static void FetchPauseManager(out PauseManager _pauseManager)
    {
        _pauseManager = GameObject.FindAnyObjectByType<PauseManager>();
    }
    #endregion

    #region Comparison methods
    #region Vector2
    public static bool Greater(in Vector2 _a, in Vector2 _b, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            return
            (_a.x >= _b.x
            && _a.y >= _b.y);
        }

        return
            (_a.x > _b.x
            && _a.y > _b.y);
    }
    public static bool Less(in Vector2 _a, in Vector2 _b, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            return
            (_a.x <= _b.x
            && _a.y <= _b.y);
        }

        return
            (_a.x < _b.x
            && _a.y < _b.y);
    }
    #endregion

    #region Vector3
    public static bool Greater(in Vector3 _a, in Vector3 _b, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            return
            (_a.x >= _b.x
            && _a.y >= _b.y
            && _a.z >= _b.z);
        }

        return
            (_a.x > _b.x
            && _a.y > _b.y
            && _a.z > _b.z);
    }
    public static bool Less(in Vector3 _a, in Vector3 _b, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            return
            (_a.x <= _b.x
            && _a.y <= _b.y
            && _a.z <= _b.z);
        }

        return
            (_a.x < _b.x
            && _a.y < _b.y
            && _a.z < _b.z);
    }
    #endregion

    #region Vector4
    public static bool Greater(in Vector4 _a, in Vector4 _b, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            return
            (_a.x >= _b.x
            && _a.y >= _b.y
            && _a.z >= _b.z
            && _a.w >= _b.w);
        }

        return
            (_a.x > _b.x
            && _a.y > _b.y
            && _a.z > _b.z
            && _a.w > _b.w);
    }
    public static bool Less(in Vector4 _a, in Vector4 _b, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            return
            (_a.x <= _b.x
            && _a.y <= _b.y
            && _a.z <= _b.z
            && _a.w <= _b.w);
        }

        return
            (_a.x < _b.x
            && _a.y < _b.y
            && _a.z < _b.z
            && _a.w < _b.w);
    }
    #endregion

    #region Color
    public static bool Greater(in Color _a, in Color _b, in bool _testAlpha = false, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            if (_testAlpha)
            {
                return
                    (_a.r >= _b.r
                    && _a.g >= _b.g
                    && _a.b >= _b.b
                    && _a.a >= _b.a);
            }

            return
                (_a.r >= _b.r
                && _a.g >= _b.g
                && _a.b >= _b.b);
        }

        if (_testAlpha)
        {
            return
                (_a.r > _b.r
                && _a.g > _b.g
                && _a.b > _b.b
                && _a.a > _b.a);
        }

        return
            (_a.r > _b.r
            && _a.g > _b.g
            && _a.b > _b.b);
    }
    public static bool Less(in Color _a, in Color _b, in bool _testAlpha = false, in bool _testEquality = true)
    {
        if (_testEquality)
        {
            if (_testAlpha)
            {
                return
                    (_a.r <= _b.r
                    && _a.g <= _b.g
                    && _a.b <= _b.b
                    && _a.a <= _b.a);
            }

            return
                (_a.r <= _b.r
                && _a.g <= _b.g
                && _a.b <= _b.b);
        }

        if (_testAlpha)
        {
            return
                (_a.r < _b.r
                && _a.g < _b.g
                && _a.b < _b.b
                && _a.a < _b.a);
        }

        return
            (_a.r < _b.r
            && _a.g < _b.g
            && _a.b < _b.b);
    }
    #endregion
    #endregion

    #region IsAtAllBut
    public static bool IsAtAllBut(in float _isAt, in float _allBut, in float _distance, in float _step, in float _divisions)
    {
        if (Mathf.Approximately(_isAt, _allBut)) { return false; }

        float increment = _step / _divisions;

        for (float val = _allBut - increment; val >= _allBut - _distance; val -= increment)
        {
            if (Mathf.Approximately(_isAt, val)) { return false; }
        }

        for (float val = _allBut + increment; val <= _allBut + _distance; val += increment)
        {
            if (Mathf.Approximately(_isAt, val)) { return false; }
        }

        return true;
    }

    #endregion
    #endregion
}
