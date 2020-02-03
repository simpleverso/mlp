# MLP - Multi Layer Perceptron
Perceptron multicapa desde cero una implementacion en c# [**Simpleverso**].

## Dependencias

```txt
- Ninguna
```

## Explicacion

Add this to your package's pubspec.yaml file:

```yaml
dependencies:
	equinox: ^0.3.3
```

## iniciacion de la red

You can install packages from the command line:

```bash
$ flutter pub get
```

## funcion de transferencia

Now in your Dart code, you can use:

```dart
import 'package:equinox/equinox.dart';
```

## propagacion

You have to replace `MaterialApp` or `CupertinoApp` with `EquinoxApp`.

```dart
class MyApp extends StatelessWidget {
	@override
	Widget build(BuildContext context) {
		return EquinoxApp(
			theme: EqThemes.defaultLightTheme,
			title: 'Flutter Demo',
			home: HomePage(),
		);
	}
}
```

Then, instead of a `Scaffold` you have to use `EqLayout`.

```dart
@override
Widget build(BuildContext context) {
	return EqLayout(
		appBar: EqAppBar(
			centerTitle: true,
			title: 'Auth test',
			subtitle: 'v0.0.3',
		),
		child: MyBody(),
	);
}
```

## retropropagacion

<p float="left">
	<img src="https://i.imgur.com/nF02pxn.jpg" width="49%" />
	<img src="https://i.imgur.com/OSEEYIj.jpg" width="49%" />
</p>

## Algoritmo Tomado Del libro:

- Redes De Neuronas Artificiales: [Repository](https://github.com/eva-design/eva)

## Contacto

**E-Mail**: myred.zapoteca@gmail.com