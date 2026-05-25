# Práctica 02: AI - Máquina de Estados Finito (FSM)

**Alumna:** XueMei Lin  
**Asignatura:** Inteligencia Artificial para Videojuegos  

---

## 🎯 Objetivo (Objetivo)
El objetivo de esta práctica es crear una Máquina de Estados Finita (FSM) para controlar el comportamiento de un NPC de forma automática. El enemigo cambiará de estado según la posición del jugador, la línea de visión y su propia vida, siguiendo estas reglas:

* **Patrol (Patrullar):** El NPC camina de forma aleatoria (`Wander`). Si ve al jugador, pasa al estado **Chase**.
* **Chase (Perseguir):** El NPC persigue al jugador (`Seek` o `Pursue`). Si se acerca lo suficiente para disparar, pasa a **Attack**. Si pierde de vista al jugador, vuelve a **Patrol**.
* **Attack (Atacar):** El NPC dispara al jugador. Si el jugador se aleja demasiado, el NPC vuelve a **Chase**. Si la vida del NPC baja de un límite, pasa a **Hide**.
* **Hide (Esconderse):** El NPC busca cobertura (`Hide` o `CleverHide`) y recupera su vida. Cuando su vida vuelve a estar alta, regresa al estado **Patrol**.