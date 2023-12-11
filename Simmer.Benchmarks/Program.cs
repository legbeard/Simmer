using BenchmarkDotNet.Running;
using Simmer.Benchmarks;

var summary = BenchmarkRunner.Run<Benchmarks>();