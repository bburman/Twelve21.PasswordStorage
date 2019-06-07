# Twelve21.PasswordStorage

This library is meant to provide utility functions for password storage. It is written in C# against .NET Core 2.2.

It currently only supports Argon2 calibration features. You can use this library to determine which parameters to pass to one of the Argon2 hashing functions.

This application is built upon the research in [this blog post](https://www.twelve21.io/how-to-choose-the-right-parameters-for-argon2/).

## Documentation

First, clone the git repository.

```
git clone https://https://github.com/bburman/Twelve21.PasswordStorage.git
```

Secondly, enter the directory and build the solution.

```
dotnet build
```

Next, enter the Twelve21.PasswordStorage subdirectory and run the Argon2 Calibration function.

```
cd ./Twelve21.PasswordStorage
dotnet run a2c
```

The application will run and show you the best results, similar to:

```
Best results:
M =  256 MB, T =    2, d = 8, Time = 0.732 s
M =  128 MB, T =    6, d = 8, Time = 0.99 s
M =   64 MB, T =   12, d = 8, Time = 0.968 s
M =   32 MB, T =   24, d = 8, Time = 0.896 s
M =   16 MB, T =   49, d = 8, Time = 0.973 s
M =    8 MB, T =   96, d = 8, Time = 0.991 s
M =    4 MB, T =  190, d = 8, Time = 0.977 s
M =    2 MB, T =  271, d = 8, Time = 0.973 s
M =    1 MB, T =  639, d = 8, Time = 0.991 s
```

For detailed help and options, add the --help option to the command line:

```
> dotnet run a2c -- --help


Usage:  a2c [options]

Options:
  -?|-h|--help      Show help information
  -t|--time         The maximum time in milliseconds it should take to calculate the password hash. Defaults to 1000.
  -p|--parallelism  The degree of parallelism. Defaults to twice the number of CPU cores.
  -i|--iterations   The minimum number of iterations. Defaults to 2.
```

## License

[MIT License](https://opensource.org/licenses/MIT)