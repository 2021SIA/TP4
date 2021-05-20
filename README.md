Unsupervised Learning Engine

## Build
Requires .Net 3.1 SDK

```console
dotnet build -c Release
```

The project can also be imported in Visual Studio 2019

## Run

```console
cd TP4\bin\Release\netcoreapp3.1\
.\TP4.exe [options]
```
Information about valid arguments can be found with the --help command

```console
.\TP4.exe --help

TP4
  Unsupervised Learning Engine

Usage:
  TP4 [options]

Options:
  --config <config>  Path to the configuration file
  --version          Show version information
  -?, -h, --help     Show help and usage information
```

## Configuration

```console
.\TP4.exe --config config.yaml
```

The configuration file must have the following format:

```console
network: <oja|kohonen|hopfield>
patterns: <hopfield patterns path>
pattern_rows: <pattern rows> (default = 5)
pattern_columns: <pattern columns> (default = 5)
test_pattern: <hopfield testing pattern> (default = 0)
noise: <hopfield testing pattern noise> (default = 0.2)
learning_rate: <oja learning rate> (default = 0.01)
csv: <csv file> (only for network: oja|kohonen)
epochs: <epochs> (default = 1000)
kohonen_k: <kohonen k map size> (only for network: kohonen. default = 4)
```



