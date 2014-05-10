using UnityEngine;

public class AreaOfEffectDamageScript : ProjectileBaseScript
{
    public Vector3 targetScale;
    private Vector3 initScale;
    public float growSpeed;
    private Color transparentColor;
    public float fadeSpeed;
    public Material originalMaterial;

    public override void Awake()
    {
        base.Awake();
        damageLevel = new int[3]
        { 
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RocketLevel0],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RocketLevel1],
            KBConstants.AbilityConstants.DAMAGE_VALUES[KBConstants.AbilityConstants.type.RocketLevel2]
        };
    }

    public override void Start()
    {
        base.Start();
        initScale = transform.localScale;
        transparentColor = new Color(originalMaterial.color.r, originalMaterial.color.g, originalMaterial.color.b, 0);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        gameObject.transform.localScale = Vector3.Lerp(transform.localScale, targetScale, growSpeed * Time.deltaTime);
        gameObject.renderer.material.color = Color.Lerp(renderer.material.color, transparentColor, fadeSpeed * Time.deltaTime);

        if (gameObject.transform.localScale.x > targetScale.x * 0.95)
        {
            Reset();
        }
    }

    public override void Init(KBPlayer _owner)
    {
        base.Init(_owner);
        lifetime = 10;
        transform.localScale = initScale;
        renderer.material.color = originalMaterial.color;
    }
}