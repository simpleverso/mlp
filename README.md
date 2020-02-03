# MLP - Multi Layer Perceptron
Perceptron multicapa desde cero una implementacion en c# [**Eva Design**](https://eva.design).

#Simpleverso
[<img src="https://i.imgur.com/oMcxwZ0.png" alt="Eva Design System" height="20px" />](https://eva.design)
[![Pub](https://img.shields.io/pub/vpre/equinox.svg)](https://pub.dev/packages/equinox)


## Screenshots

<p float="left">
	<img src="https://i.imgur.com/nF02pxn.jpg" width="49%" />
	<img src="https://i.imgur.com/OSEEYIj.jpg" width="49%" />
	<img src="https://i.imgur.com/alMhkL8.jpg" width="49%" />
	<img src="https://i.imgur.com/z7UEPAM.jpg" width="49%" />
</p>


### Explicacion

Add this to your package's pubspec.yaml file:

```yaml
dependencies:
	equinox: ^0.3.3
```

### iniciacion de la red

You can install packages from the command line:

```bash
$ flutter pub get
```

### funcion de transferencia

Now in your Dart code, you can use:

```dart
import 'package:equinox/equinox.dart';
```

### propagacion

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

### retropropagacion

**Equinox** 


## Customization

Customization is done using [stylist](https://github.com/kekland/stylist). I will write a guide on styling your app soon.

## Other Eva Design implementations

- [**Angular**](https://github.com/akveo/nebular)
- [**React Native**](https://github.com/akveo/react-native-ui-kitten)


## Algoritmo Tomado Del libro:

- Redes De Neuronas Artificiales: [Repository](https://github.com/eva-design/eva)

## Contacto

**E-Mail**: myred.zapoteca@gmail.com