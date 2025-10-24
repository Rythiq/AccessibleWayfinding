using AccessibleWayfinding;
using UnityEngine;
using UnityEngine.UI;

public class SimplePointerCue : MonoBehaviour, ICue // THIS IS A 2D CUE, WILL NOT WORK IN 3D
{
    
    public ICue.CueType Type => ICue.CueType.Visual;
    public bool IsActive => image.enabled;
    
    public Image image;
    private Transform target;
    
    void Start()
    {
        image.enabled = false;
        WayfindingManager.Instance.RegisterCue(this);
    }

    void Update()
    {
        if (target && image.enabled)
        {
            var dir =  (target.position - WayfindingManager.Instance.playerTransform.position).normalized;
            image.transform.rotation = Quaternion.Euler(0,0,(Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg)-90);
        }
    }

    public void Activate(Transform target)
    {
        this.target = target;
        image.enabled = true;
    }

    public void Deactivate()
    {
        image.enabled = false;
    }
}
