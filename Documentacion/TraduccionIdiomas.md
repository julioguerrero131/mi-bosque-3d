[⬅️Volver](../README.md)

# 🌐 Sistema de Traducción de Idiomas

El sistema de traducción implementado en el juego tiene como objetivo principal facilitar la adaptación de todos los textos a diferentes idiomas sin necesidad de modificar manualmente la interfaz o el código. Para ello se diseñó un enfoque basado en claves y archivos JSON, administrado de forma centralizada por el **`LanguageManager`**, lo que garantiza consistencia y escalabilidad.

Los textos visibles en la interfaz, diálogos o menús se vinculan a estas claves mediante scripts especializados (**`TranslatableText`**, **`TranslatableTMP2`**, **`DialogueTrigger`** y **`DialogueManager`**), que se encargan de mostrar la traducción correcta en función del idioma activo. Además, el sistema permite cambiar de idioma en tiempo real y ofrece la flexibilidad de usar tanto claves de traducción como texto directo cuando sea necesario.

En conjunto, este enfoque asegura un manejo ordenado, flexible y mantenible de la localización dentro del proyecto, sentando una base sólida para futuras expansiones multilingües del juego.

## LanguageManager

El script **`LanguageManager`** implementa un sistema de gestión de idiomas en Unity para un videojuego. Permite cargar y cambiar dinámicamente los textos mostrados en la interfaz de usuario a partir de archivos JSON ubicados en la carpeta Resources. Su diseño facilita que la experiencia pueda adaptarse a distintos idiomas sin necesidad de modificar el código fuente principal.

## 📝 Pasos para Añadir Textos

### Añadir el texto en el archivo JSON
El primer paso para agregar un nuevo texto traducible es editar el archivo JSON correspondiente al idioma (por ejemplo, `textos_espanol.json`). Los archivos JSON siguen una jerarquía de secciones, y dentro de cada sección hay pares clave–valor, donde la clave identifica el texto y el valor es la traducción.

Un ejemplo son los textos de los botones de diferentes menús, se los divide por escenas o por partes clave de lo que es la lógica del juego.

Los tres archivos (`textos_espanol.json`, `textos_english.json` y `textos_portugues.json`) deben tener la misma estructura y las mismas claves para no tener problemas de que algún texto no se muestre en cierto idioma en específico.

### Procesar el texto en LanguageManager.cs
El **`LanguageManager`** está diseñado como un Singleton, que sirve para recorrer todas las secciones del JSON y añadir automáticamente los textos al diccionario interno (`textos`). Al añadir una nueva clave-valor, es necesario ingresarlo dentro de la función `CargarIdioma()`, que se encarga de guardar todos los textos del JSON en el diccionario textos.

Para las claves que se ingresan en el diccionario, se sigue la convención de ingresar las claves, indicando jerarquía de las claves mediante puntos `<clave_padre>.<clave_hijo>`. La forma de nombrarla no es necesaria que siga el mismo formato, sin embargo, es para poder ubicar de mejor manera los diferentes textos.

### Añadir a la Estructura de Clases
Debe seguirse la estructura de los JSON para la creación de las clases para mapear los textos. **`DatosIdioma`** es la clase principal, y de ella se van dividiendo para las distintas escenas, botones, avisos, etc.

Tanto el JSON como las clases siguen una misma estructura. Con comentarios se indica en el código qué clase pertenece a qué parte del JSON.

Es importante recalcar que los nombres de las propiedades de las clases deben tener el mismo nombre, escritos de la misma forma. Por ejemplo, dentro del JSON hay una clave llamada "botones", que tiene los textos de todos los botones que hay dentro del juego, y como se puede ver en la captura, existe una propiedad con el mismo nombre, "botones".

### Usar el diccionario textos
El diccionario es utilizado siempre llamando a la instancia del **`LanguageManager`** para obtener el texto según la clave que se puso. En los siguientes puntos se describe como se usa.

## 📄 Textos en Scripts

Una de las formas de uso es de forma directa dentro de algún script, obteniendo la instancia del **`LanguageManager`** para buscar un texto en específico y usarlo.

Esta se usa principalmente para textos que se cargan dentro scripts, fuera de la interfaz de Unity.

Esto ocurre en escenas como la del Tutorial, que manejan los textos de forma diferente a las otras escenas. También se utiliza para la creación dentro del juego de las misiones y logros que tendrá que completar el jugador, dentro del script **`LogrosGlobales`**.

## 🔤 Componentes Text

El script **`TranslatableText.cs`** se utiliza cuando el objeto en escena contiene un componente `UnityEngine.UI.Text` (el texto clásico de la UI en Unity). Su función es suscribirse al evento de cambio de idioma y actualizar automáticamente el texto mostrado en pantalla según la clave asignada.

### Campos principales
- **`public string clave;`**  
  Es la clave definida en el archivo de idiomas (JSON). A través de ella se obtiene el texto traducido.
- **`private Text textoUI;`**  
  Referencia al componente Text del objeto.

### Funciones principales
- **`Awake()`**  
  Busca el componente Text en el objeto. Si no lo encuentra, lanza un error en la consola.
- **`OnEnable()`**  
  Si `LanguageManager.Instancia` ya existe, se suscribe al evento `OnIdiomaCambiado` y actualiza el texto inmediatamente. Si aún no existe, comienza una corrutina (`EsperarLanguageManager`) para esperar hasta que se inicialice.
- **`OnDisable()`**  
  Se desuscribe del evento para evitar errores o referencias colgadas.
- **`ActualizarTexto()`**  
  Consulta al `LanguageManager` con la clave y asigna el texto traducido al componente Text.
- **`EsperarLanguageManager()`**  
  Corrutina que espera hasta que `LanguageManager` esté disponible. Una vez listo, se suscribe al evento y actualiza el texto.

### 🔧 Uso
1. Añadir el script **`TranslatableText.cs`** a un objeto que contenga un componente Text.
2. Definir la clave correspondiente en el inspector.
3. Al ejecutar y cambiar de idioma, el texto se actualizará automáticamente.

## 🎨 Componentes TextMesh, TextMeshPro y TextMeshProUGUI

En cambio, el script **`TranslatableTMP2.cs`** es una versión más flexible, pensado para usarse con TextMeshPro o incluso con el antiguo TextMesh 3D. Permite cubrir distintos tipos de componentes de texto sin necesidad de tener múltiples scripts separados.

### Campos principales
- **`public string clave;`**  
  Clave de idioma a traducir.
- **`private TextMeshProUGUI textoUI;`**  
  Para textos dentro de un Canvas usando TextMeshPro.
- **`private TextMeshPro texto3D;`**  
  Para textos 3D en el mundo.
- **`private TextMesh textoMesh;`**  
  Para el componente clásico TextMesh.

### Funciones principales
- **`Awake()`**  
  Intenta obtener las tres variantes de texto posibles. Valida que al menos una esté presente, de lo contrario muestra un error en consola.
- **`OnEnable()`**  
  Igual que en `TranslatableText.cs`, se suscribe al evento de idioma o espera con una corrutina.
- **`OnDisable()`**  
  Se desuscribe del evento.
- **`ActualizarTexto()`**  
  Obtiene el nuevo texto desde `LanguageManager`. Asigna el valor en cualquiera de los componentes que se hayan encontrado (`TextMeshProUGUI`, `TextMeshPro`, `TextMesh`).
- **`EsperarLanguageManager()`**  
  Corrutina idéntica a la del otro script, espera a que el `LanguageManager` esté disponible.

### 🔧 Uso
1. Añadir el script **`TranslatableTMP2.cs`** al objeto con texto.
2. Asegurarse de que el objeto tenga uno de los siguientes:
   - `TextMeshProUGUI` (Canvas UI).
   - `TextMeshPro` (texto 3D TMP).
   - `TextMesh` (clásico).
3. Definir la clave en el inspector.
4. El texto se traducirá automáticamente al cambiar de idioma.

## 💬 Componente DialogueTrigger

Para los diálogos que se muestran mediante un canvas, para indicar información sobre los retos que deben completar los jugadores, o información adicional que se quiere mostrar, se utiliza activadores que llaman al **`DialogueTrigger`** y este utiliza dos scripts más para funcionar, estos dos scripts que son **`DialogueManager`** y **`LanguageManager`**.

**`DialogueManager`** es el encargado de controlar la lógica de los diálogos en el juego: mostrar los títulos, las frases, las imágenes y las expresiones de los personajes. Además, se asegura de que cada texto mostrado esté traducido al idioma actual del jugador. Esto lo hace a través del método `LocalizeDialogue`, que se apoya directamente en el **`LanguageManager`**.

**`DialogueTrigger`** actúa como puente: su única función es llamar a `DialogueManager.StartDialogue` y pasarle la información del diálogo que debe mostrarse. La localización ocurre solo en **`DialogueManager`**.

### Relación con LanguageManager

El **`LanguageManager`** es utilizado dentro de **`DialogueManager`** en el método `LocalizeDialogue(Dialogue dialogue)`, cuya responsabilidad es:

- Verificar si `LanguageManager.Instancia` existe (si no, se inicializa automáticamente).
- Tomar cada clave de texto en `Dialogue.title` y `Dialogue.sentences`.
- Pasar esa clave a `LanguageManager.Instancia.ObtenerTexto(clave)`.
- Sustituir la clave por la traducción correspondiente.
- Si no se encuentra traducción, registrar un warning en consola.

Esto asegura que los diálogos no dependan de strings estáticos, sino de claves de idioma dinámicas que se resuelven en tiempo de ejecución.

Estas claves fueron definidas como ya se específico en el **`LanguageManager`**, y se utilizan las claves definidas en el diccionario `textos`, de este mismo script.

### 🔗 Integración y Ventajas

El uso combinado de **`DialogueTrigger`**, **`DialogueManager`** y **`LanguageManager`** resulta conveniente porque separa responsabilidades: un script dispara el evento, otro controla la lógica del diálogo y el último gestiona las traducciones. Esto permite que los diálogos sean fáciles de escalar, mantener y traducir sin duplicar código ni modificar la lógica central.

En los diálogos, las propiedades como `titles` y `sentences` aceptan tanto claves de traducción como texto escrito directamente. Esto brinda flexibilidad, ya que los desarrolladores pueden decidir si un fragmento debe estar vinculado al sistema de localización o si puede mostrarse como un texto fijo sin necesidad de JSON.
