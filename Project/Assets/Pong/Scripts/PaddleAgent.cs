using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using Random = UnityEngine.Random;

public class PaddleAgent : Agent {

    // Referencia al transform del objeto bola
    [SerializeField] private Transform ballTransform;
    // Velocidad de movimiento del agente
    [SerializeField] private float moveSpeed = 5f;

    // Método que se llama al comenzar un episodio
    public override void OnEpisodeBegin() {
        // Coloca al agente en una posición aleatoria en el eje Y
        transform.localPosition = new Vector3(transform.localPosition.x, Random.Range(-3.7f, 2.5f), 0);
        // Coloca la bola en la posición inicial
        ballTransform.localPosition = new Vector3(0, 0, 0);
    }

    // Método para recopilar observaciones del entorno
    public override void CollectObservations(VectorSensor sensor) {
        // Agrega la posición local del agente como observación
        sensor.AddObservation(transform.localPosition);
        // Agrega la posición local de la bola como observación
        sensor.AddObservation(ballTransform.localPosition);
    }

    // Método que permite controlar al agente manualmente
    public override void Heuristic(in ActionBuffers actionsOut) {
        // Acciones continuas del agente
        ActionSegment<float> continuousActions = actionsOut.ContinuousActions;
        // Obtiene la entrada del eje vertical
        continuousActions[0] = Input.GetAxisRaw("Vertical");
    }

    // Método que se llama cuando el agente recibe una acción
    public override void OnActionReceived(ActionBuffers actions) {
        // Obtiene el movimiento en el eje Y
        float moveY = actions.ContinuousActions[0];
        // Mueve al agente en el eje Y multiplicado por la velocidad y el tiempo transcurrido
        transform.localPosition += new Vector3(0, moveY, 0) * Time.deltaTime * moveSpeed;
        // Penalización por moverse demasiado.
        AddReward(-0.001f);
    }

    // Método que se llama cuando el agente colisiona con otro objeto 2D
    private void OnCollisionEnter2D(Collision2D collision) {
        // Si la colisión es con un objeto con la etiqueta "Ball"
        if (collision.gameObject.CompareTag("Ball")) {
            // Recompensa por golpear la bola
            AddReward(1f);
        }
    }
}
