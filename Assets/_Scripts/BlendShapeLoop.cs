using UnityEngine;

public class BlendShapeLoop : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh mesh;
    int blendShapeCount;
    int playIndex = 0;

    float timer = 0f;
    public float animationSpeed = 0.05f;

    private void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        blendShapeCount = mesh.blendShapeCount;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= animationSpeed)
        {
            timer -= animationSpeed;

            if (playIndex > 0)
                skinnedMeshRenderer.SetBlendShapeWeight(playIndex - 1, 0f);
            if (playIndex == 0)
                skinnedMeshRenderer.SetBlendShapeWeight(blendShapeCount - 1, 0f);

            skinnedMeshRenderer.SetBlendShapeWeight(playIndex, 100f);

            playIndex++;
            if (playIndex > blendShapeCount - 1)
                playIndex = 0;
        }
    }
}
