[⬅️Volver](../README.md)
# 🎓 Reducir el tiempo de Celebración final

Lo que se buscaba era que el diploma y la celebración sean más rápidas, ahora el certificado es más veloz en desaparecer y los créditos no salen montados sobre el diploma.
##  FinalManager
Este componente de Unity controla la **secuencia final del juego**, incluyendo la animación de la cámara, la aparición del diploma, la transición hacia los créditos y la desactivación de elementos de la escena.  

---

### Relación con otros elementos

- **Cámara (`camera`)**: se mueve automáticamente hacia el objetivo definido en `target`.  
- **Polo (`polo`)**: personaje animado que cambia de estado al terminar los aplausos.  
- **NPCs / Globos / Confetti**: elementos que se desactivan antes de mostrar los créditos.  
- **Audio (`audioClap`)**: sonido de aplausos reproducido al inicio de la secuencia.  
- **Canvas**: contiene el fondo y el diploma que se mostrarán en la secuencia final.  
- **Diploma (`miniCertificado` y `diploma`)**: representa el logro del jugador, escalado progresivamente hasta alcanzar el tamaño objetivo.  
- **Credits**: objeto que activa la animación de créditos.  
- **PlayerData**: usado para cargar el nombre del jugador en el diploma.  


### Funciones principales

- **`Start()`**  
  Inicializa referencias, carga datos del jugador y ubica los elementos del canvas (fondo y diploma).  

- **`Update()`**  
  Controla el ciclo de la secuencia:  
  - Movimiento automático de cámara.  
  - Animación del personaje Polo.  
  - Activación del diploma.  
  - Transición hacia créditos si corresponde.  

- **`cameraAutoMove()`**  
  Mueve la cámara de forma progresiva hacia la posición del objetivo (`target`).  

- **`statePolo()`**  
  Cambia la animación del personaje **Polo** cuando terminan los aplausos.  

- **`activeDiplome()`**  
  Maneja la aparición del diploma:  
  - Oculta el mini certificado.  
  - Muestra el diploma con escalado progresivo.  
  - Lanza los créditos una vez alcanzado el tamaño objetivo.  

- **`StartCreditsAfter(float waitTime)`** *(corrutina)*  
  Espera un tiempo definido antes de iniciar la transición a créditos.  

- **`transitionToCredits()`**  
  - Detiene los aplausos.  
  - Desvanece el diploma progresivamente.  
  - Activa el fondo con transparencia creciente.  
  - Cuando el fondo alcanza opacidad completa, desactiva elementos de la escena y muestra los créditos.  

- **`playCredits()`**  
  Lanza la animación de los créditos.  

- **`offElements()`**  
  Desactiva NPCs, globos, confetti y al personaje Polo.  

- **`loadData()`**  
  Obtiene los datos del jugador desde el `GameManager` y asigna su nombre en el diploma.  


### ️ Variables importantes

- `currentSpeed`: velocidad de movimiento de la cámara.  
- `diplomaScaleTarget`: escala final del diploma.  
- `diplomaScaleSpeed`: rapidez con que se alcanza la escala final.  
- `pauseBeforeCredits`: tiempo de espera antes de iniciar créditos.  
- `fadingToCredits`: controla si está activa la transición hacia créditos.  
- `creditsLaunched`: evita que los créditos se lancen más de una vez.  



### 📊 Flujo básico de la secuencia

1. La cámara se mueve automáticamente hacia el objetivo.  
2. El personaje Polo cambia de estado cuando terminan los aplausos.  
3. Se muestra el diploma, escalándose progresivamente.  
4. Tras una breve pausa, comienza la transición hacia los créditos:  
   - Se desvanece el diploma.  
   - Aparece el fondo.  
   - Se desactivan NPCs, globos y confetti.  
5. Se activa la animación de créditos.  
