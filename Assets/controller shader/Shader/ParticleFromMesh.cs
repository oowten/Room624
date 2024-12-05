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
            particles[i].startSize = 0.1f; // �ɤl�j�p
            particles[i].startColor = Color.white; // �ɤl�C��
            particles[i].startLifetime = 1f; // �ɤl���ͩR�g��
        }

        particleSystem.SetParticles(particles, particles.Length);
    }
}
