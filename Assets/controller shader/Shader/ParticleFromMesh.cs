using UnityEngine;

public class ParticleFromMesh : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public MeshFilter meshFilter;

    void Start()
    {
        Mesh mesh = meshFilter.sharedMesh;
        Vector3[] vertices = mesh.vertices;

        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            particles[i].position = meshFilter.transform.TransformPoint(vertices[i]);
            particles[i].startSize = 0.1f; // 粒子大小
            particles[i].startColor = Color.white; // 粒子顏色
            particles[i].startLifetime = 1f; // 粒子的生命週期
        }

        particleSystem.SetParticles(particles, particles.Length);
    }
}
