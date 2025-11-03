# 🧠 Traductor de Código (MiniJava → JavaScript / C++)

**Traductor de Código** es una aplicación de escritorio desarrollada en **C# y WPF (.NET 8)**  
que permite **convertir código escrito en un lenguaje simple tipo MiniJava** hacia **JavaScript o C++**, 
aplicando principios de análisis léxico, sintáctico y generación de código (AST y Emitter pattern).

El proyecto fue diseñado con una arquitectura modular y moderna, 
usando **Material Design**, **MVVM Toolkit**, y **AvalonEdit** para la edición de código.

---

## 📂 Estructura del Proyecto

```
TraductorCodigo/
├─ Transpiler.Core/          # Núcleo del traductor
│  ├─ Lexing/                # Analizador léxico
│  ├─ Parsing/               # Analizador sintáctico (parser)
│  ├─ Emitters/              # Generadores de código (JS, C++)
│  ├─ Ast/                   # Definición del árbol de sintaxis abstracta (AST)
│  ├─ SampleCode.cs          # Código MiniJava de ejemplo
│  ├─ TargetLang.cs          # Enum con los lenguajes destino
│  └─ TranspilerFacade.cs    # Fachada para ejecutar el proceso completo
│
├─ Transpiler.Desktop/       # Aplicación WPF (interfaz Material Design)
│  ├─ Views/                 # Ventanas y controles visuales
│  ├─ ViewModels/            # Lógica de presentación (MVVM)
│  ├─ Services/              # Funciones auxiliares (abrir/guardar archivos)
│  ├─ Themes/                # Diccionarios de estilos y colores
│  └─ App.xaml               # Configuración de recursos y tema
│
├─ Transpiler.Tests/         # Pruebas unitarias (xUnit)
│  ├─ ParserTests.cs
│  ├─ JsEmitterTests.cs
│  ├─ CppEmitterTests.cs
│  └─ MarkdownDocTests.cs
│
└─ README.md
```

---

## ⚙️ Requisitos

- **Visual Studio 2022** o superior  
- **.NET 8 SDK**  
- **Windows 10/11**
- Paquetes NuGet usados:
  - `MaterialDesignThemes` (v5.x)
  - `MaterialDesignColors`
  - `AvalonEdit`
  - `CommunityToolkit.Mvvm`
  - `xUnit` (para pruebas)

---

## 🚀 Ejecución

1. Abre la solución en Visual Studio  
2. Establece **Transpiler.Desktop** como proyecto de inicio  
3. Compila (Ctrl + Shift + B)  
4. Ejecuta (F5)

Aparecerá una ventana con:
- **Editor izquierdo:** para escribir código MiniJava  
- **Panel derecho:** donde se muestra el código traducido  
- **Barra superior:** permite abrir, guardar o traducir archivos  

---

## 🧩 Ejemplo de uso

### 🔹 Entrada (MiniJava)
```java
func main() {
    var x = 2 + 3 * 4;
    int y = 10;
    if (x > y) { print("x es mayor"); } else { print("y es mayor"); }
    while (y < 15) { y = y + 1; }
    return;
}
```

### 🔸 Salida (JavaScript)
```js
function main() {
    let x = 2 + 3 * 4;
    let y = 10;
    if (x > y) {
        console.log("x es mayor");
    } else {
        console.log("y es mayor");
    }
    while (y < 15) {
        y = y + 1;
    }
    return;
}
```

### 🔸 Salida (C++)
```cpp
#include <bits/stdc++.h>
using namespace std;

int main() {
    auto x = 2 + 3 * 4;
    int y = 10;
    if (x > y) {
        cout << "x es mayor" << endl;
    } else {
        cout << "y es mayor" << endl;
    }
    while (y < 15) {
        y = y + 1;
    }
    return 0;
}
```

---

## 🧠 Arquitectura

### 🏗️ **Core (Transpiler.Core)**
Implementa la lógica del traductor:
- **Lexer:** convierte texto en tokens  
- **Parser:** construye el AST (árbol de sintaxis abstracta)  
- **Emitters:** recorren el AST para generar código destino (JS o C++)  
- **Facade:** expone un método simple `TranspilerFacade.Transpile(source, target)`

### 🎨 **UI (Transpiler.Desktop)**
Interfaz moderna con **Material Design**:
- Editor con **AvalonEdit** para código fuente  
- Soporte para abrir/guardar archivos  
- Modo claro/oscuro configurable  
- Barra superior con botones y combobox de lenguaje

### 🧪 **Tests (Transpiler.Tests)**
Automatiza la verificación del núcleo:
- Valida el parser y los generadores de código  
- Asegura la estabilidad del sistema con cada cambio

---

## 🧰 Comandos principales

| Comando | Descripción |
|----------|--------------|
| `TranspilerFacade.Transpile(source, TargetLang.JavaScript)` | Transforma código MiniJava a JavaScript |
| `TranspilerFacade.Transpile(source, TargetLang.Cpp)` | Transforma código MiniJava a C++ |
| `FileService.OpenText()` | Abre un archivo desde el disco |
| `FileService.SaveText()` | Guarda el resultado traducido |

---

## 🧾 Estructura MVVM

| Capa | Ejemplo de clase | Rol |
|------|-------------------|-----|
| **View** | `MainWindow.xaml` | Interfaz del usuario |
| **ViewModel** | `MainViewModel.cs` | Lógica de presentación, comandos |
| **Model/Core** | `Parser`, `JsEmitter`, `CppEmitter` | Lógica de negocio (traducción) |

---

## 💡 Extensiones futuras

- 🌈 Resaltado de sintaxis MiniJava  
- 📦 Exportación de resultados a HTML o PDF  
- 🧩 Integración con IA para sugerir correcciones de código  
- 🧠 Soporte para más lenguajes destino (Python, C#, TypeScript)

---

## 👨‍💻 Autor

**Proyecto desarrollado por:**  
> *Eliana Gómez*  
> 💻 Estudiante de Ingeniería de Software  
> 📍 República Dominicana  

---

## 🏁 Licencia

Este proyecto se distribuye bajo la licencia **MIT**, lo que permite su uso, copia, modificación y distribución libre con atribución.

---

> _“Traducir código es entender la lógica más allá del lenguaje.”_ 💡
