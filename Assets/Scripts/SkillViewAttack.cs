using UIExtensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkillViewAttack : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private Button _removeSkill;
    [SerializeField] private Image _shirt;

    private bool _isInitialized;
    public bool Initialized => _isInitialized;
    private SkillStaticData _skillStaticData;
    public event UnityAction<SkillStaticData, SkillViewAttack> RemoveSkillsButton;
    public SkillStaticData SkillStaticData => _skillStaticData;

    private void OnEnable()
    {
        if (_isInitialized == false)
            return;

        _removeSkill.Add(OnClick);
    }

    private void OnDisable() => _removeSkill.Remove(OnClick);
    public void Show() => _shirt.Activate();
    public void Hide() => _shirt.Deactivate();

    public void Initialize(SkillStaticData skillStaticData)
    {
        _skillStaticData = skillStaticData;
        _icon.sprite = skillStaticData.UIIcon;
        
        _isInitialized = true;
        OnEnable();
    }

    public void RemoveSkill()
    {
        _icon.sprite = null;
        _isInitialized = false;
        _skillStaticData = null;
        Show();
    }

    public void RemoveAttack() => _removeSkill.interactable = false;
    public void UpAttack() => _removeSkill.interactable = true;
    public void SetInitialized() => _isInitialized = !_isInitialized;
    private void OnClick() => RemoveSkillsButton?.Invoke(_skillStaticData, this);
}