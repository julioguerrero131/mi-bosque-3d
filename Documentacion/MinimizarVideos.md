[⬅️Volver](../README.md)
# FIX: 🎬 Minimizador de videos
Se necesitaba minimizar los videos que aparecen dentro del bosque de manera correcta, porque se seguía escuchando a pesar de que la imagen desaparecía.

Este componente de Unity se encarga de **gestionar la minimización de los videos en pantalla**, deteniendo su reproducción y ocultando el canvas que los contiene.  

---

##  Relación con otros elementos

- **`VideoPlayer`**  
  - Componente de Unity que controla la reproducción del video.  
- **`AudioSource`**  
  - Controla la reproducción del audio asociado al video.  
- **Canvas del video (`videoCanvas` / `videoCanvas2`)**  
  - GameObjects que contienen los videos y se activan/desactivan al minimizar.  



##  Funciones principales

- **`OnMinimizar()`**  
  - Detiene el **primer video** (`videoPlayer`) y su audio.  
  - Desactiva el canvas (`videoCanvas`).  
  - Marca la bandera `VideoUnoMinimizado = true`.  
  - Muestra en consola un log de confirmación.  

- **`OnMinimizarVideo2()`**  
  - Detiene el **segundo video** (`videoPlayer2`) y su audio.  
  - Desactiva el canvas (`videoCanvas2`).  
  - Muestra en consola un log de confirmación.  

- **`minimizadorVideos()`**  
  - Lógica general para decidir qué video minimizar:  
    - Si el primer video **no ha sido minimizado**, ejecuta `OnMinimizar()`.  
    - Si ya fue minimizado, ejecuta `OnMinimizarVideo2()`.  


##  Variables expuestas

- `videoCanvas` y `videoCanvas2`: contenedores visuales donde se muestran los videos.  
- `videoPlayer` y `videoPlayer2`: controladores de video.  
- `audioSource` y `audioSource2`: controladores de audio.  
- `VideoUnoMinimizado`: bandera interna que indica si el primer video ya fue minimizado.  



## 📊 Flujo básico de uso

1. El usuario dispara la acción de minimizar (ej. botón en UI).  
2. El script verifica si el **primer video** sigue activo.  
3. Si está activo → ejecuta `OnMinimizar()`.  
   - Se detiene el video 1 y su audio.  
   - Se oculta el `videoCanvas`.  
   - Se actualiza la bandera `VideoUnoMinimizado`.  
4. Si el primer video ya fue minimizado → ejecuta `OnMinimizarVideo2()`.  
   - Se detiene el video 2 y su audio.  
   - Se oculta el `videoCanvas2`.  



  
