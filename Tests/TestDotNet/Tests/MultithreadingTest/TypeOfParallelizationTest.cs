using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.MultithreadingTest;


#region i7

//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19045
//Intel Core i7-8700K CPU 3.70GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//.NET SDK=7.0.103
//  [Host]     : .NET 6.0.20 (6.0.2023.32017), X64 RyuJIT
//  DefaultJob : .NET 6.0.20 (6.0.2023.32017), X64 RyuJIT
//
//
//|               Method |    Count |             Mean |          Error |         StdDev |           Median |
//|--------------------- |--------- |-----------------:|---------------:|---------------:|-----------------:|
//|        CallOneThread |      100 |         2.150 us |      0.0036 us |      0.0032 us |         2.151 us |
//
//|         ParallelFor1 |      100 |         3.237 us |      0.0531 us |      0.0471 us |         3.235 us |
//|           TaskBased1 |      100 |         3.401 us |      0.0671 us |      0.1192 us |         3.423 us |
//
//|         ParallelFor2 |      100 |         3.708 us |      0.0094 us |      0.0088 us |         3.709 us |
//|           TaskBased2 |      100 |         2.622 us |      0.0306 us |      0.0287 us |         2.618 us |
//
//|         ParallelFor4 |      100 |         4.004 us |      0.0664 us |      0.0621 us |         4.018 us |
//|           TaskBased4 |      100 |         3.248 us |      0.0238 us |      0.0223 us |         3.246 us |
//
//|         ParallelFor6 |      100 |         4.062 us |      0.0101 us |      0.0095 us |         4.063 us |
//|           TaskBased6 |      100 |         4.422 us |      0.0355 us |      0.0332 us |         4.414 us |
//
//|         ParallelFor8 |      100 |         4.278 us |      0.0193 us |      0.0181 us |         4.274 us |
//|           TaskBased8 |      100 |         5.635 us |      0.0166 us |      0.0156 us |         5.637 us |
//
//| ParallelForCoreCount |      100 |         4.975 us |      0.0125 us |      0.0111 us |         4.976 us |
//|   TaskBasedCoreCount |      100 |         8.219 us |      0.1384 us |      0.1294 us |         8.266 us |
//
//
//|        CallOneThread |     1000 |        21.571 us |      0.0436 us |      0.0387 us |        21.556 us |
//
//|         ParallelFor1 |     1000 |        25.092 us |      0.3111 us |      0.2910 us |        25.203 us |
//|           TaskBased1 |     1000 |        29.922 us |      0.2368 us |      0.2215 us |        29.968 us |
//
//|         ParallelFor2 |     1000 |        13.926 us |      0.1008 us |      0.0894 us |        13.900 us |
//|           TaskBased2 |     1000 |        19.647 us |      0.2238 us |      0.2093 us |        19.671 us |
//
//|         ParallelFor4 |     1000 |        13.920 us |      0.0619 us |      0.0579 us |        13.926 us |
//|           TaskBased4 |     1000 |        16.010 us |      0.3173 us |      0.4846 us |        15.963 us |
//
//|         ParallelFor6 |     1000 |        12.157 us |      0.0748 us |      0.0699 us |        12.182 us |
//|           TaskBased6 |     1000 |        10.404 us |      0.0860 us |      0.0804 us |        10.386 us |
//
//|         ParallelFor8 |     1000 |        11.778 us |      0.0875 us |      0.0819 us |        11.760 us |
//|           TaskBased8 |     1000 |         9.990 us |      0.1914 us |      0.2683 us |         9.992 us |
//
//| ParallelForCoreCount |     1000 |        11.175 us |      0.1623 us |      0.1518 us |        11.186 us |
//|   TaskBasedCoreCount |     1000 |        10.902 us |      0.2016 us |      0.1787 us |        10.860 us |
//
//
//|        CallOneThread |     5000 |       112.941 us |      0.2168 us |      0.1922 us |       112.852 us |
//
//|         ParallelFor1 |     5000 |       122.847 us |      0.3881 us |      0.3631 us |       122.825 us |
//|           TaskBased1 |     5000 |       124.544 us |      0.3816 us |      0.3569 us |       124.466 us |
//
//|         ParallelFor2 |     5000 |        66.143 us |      0.0764 us |      0.1167 us |        66.111 us |
//|           TaskBased2 |     5000 |        67.654 us |      0.1560 us |      0.1459 us |        67.658 us |
//
//|         ParallelFor4 |     5000 |        57.572 us |      0.3356 us |      0.3139 us |        57.560 us |
//|           TaskBased4 |     5000 |        56.357 us |      0.2271 us |      0.2013 us |        56.421 us |
//
//|         ParallelFor6 |     5000 |        45.249 us |      0.1616 us |      0.1512 us |        45.231 us |
//|           TaskBased6 |     5000 |        48.667 us |      0.0415 us |      0.0388 us |        48.664 us |
//
//|         ParallelFor8 |     5000 |        35.095 us |      0.2002 us |      0.1872 us |        35.100 us |
//|           TaskBased8 |     5000 |        33.699 us |      0.5633 us |      0.4704 us |        33.859 us |
//
//| ParallelForCoreCount |     5000 |        27.967 us |      0.1779 us |      0.1664 us |        28.040 us |
//|   TaskBasedCoreCount |     5000 |        27.626 us |      0.2947 us |      0.2757 us |        27.619 us |
//
//
//|        CallOneThread |    10000 |       227.632 us |      0.8967 us |      0.7949 us |       227.223 us |
//
//|         ParallelFor1 |    10000 |       242.291 us |      0.3508 us |      0.3110 us |       242.219 us |
//|           TaskBased1 |    10000 |       292.890 us |      2.5186 us |      2.3559 us |       293.360 us |
//
//|         ParallelFor2 |    10000 |       126.704 us |      0.2159 us |      0.1914 us |       126.666 us |
//|           TaskBased2 |    10000 |       128.082 us |      0.8924 us |      0.7452 us |       127.860 us |
//
//|         ParallelFor4 |    10000 |        99.909 us |      1.2132 us |      1.1349 us |       100.109 us |
//|           TaskBased4 |    10000 |        89.126 us |      2.5010 us |      7.3743 us |        85.762 us |
//
//|         ParallelFor6 |    10000 |        78.133 us |      1.5361 us |      1.8864 us |        78.051 us |
//|           TaskBased6 |    10000 |        75.516 us |      0.1799 us |      0.1683 us |        75.444 us |
//
//|         ParallelFor8 |    10000 |        60.608 us |      0.0942 us |      0.0881 us |        60.591 us |
//|           TaskBased8 |    10000 |        65.295 us |      0.8049 us |      0.7529 us |        65.409 us |
//
//| ParallelForCoreCount |    10000 |        47.793 us |      0.7909 us |      0.7398 us |        48.080 us |
//|   TaskBasedCoreCount |    10000 |        43.776 us |      0.1861 us |      0.1554 us |        43.780 us |
//
//
//|        CallOneThread |    50000 |     1,144.982 us |     10.0916 us |      9.4397 us |     1,140.024 us |
//
//|         ParallelFor1 |    50000 |     1,221.579 us |      2.8715 us |      2.5455 us |     1,220.472 us |
//|           TaskBased1 |    50000 |     1,218.240 us |      8.7658 us |      8.1995 us |     1,215.695 us |
//
//|         ParallelFor2 |    50000 |       639.632 us |      3.9655 us |      3.7093 us |       639.330 us |
//|           TaskBased2 |    50000 |       678.865 us |     13.5565 us |     17.1446 us |       671.203 us |
//
//|         ParallelFor4 |    50000 |       403.663 us |      7.9824 us |     16.3060 us |       410.669 us |
//|           TaskBased4 |    50000 |       486.670 us |     12.1588 us |     35.8505 us |       499.409 us |
//
//|         ParallelFor6 |    50000 |       364.120 us |      5.7938 us |      5.4195 us |       363.620 us |
//|           TaskBased6 |    50000 |       409.273 us |      6.1133 us |      5.7184 us |       410.033 us |
//
//|         ParallelFor8 |    50000 |       297.391 us |      5.9162 us |     11.5390 us |       295.600 us |
//|           TaskBased8 |    50000 |       291.915 us |      4.6450 us |      4.3449 us |       292.437 us |
//
//| ParallelForCoreCount |    50000 |       197.404 us |      3.6589 us |      3.2435 us |       198.160 us |
//|   TaskBasedCoreCount |    50000 |       193.397 us |      1.7795 us |      1.5775 us |       193.623 us |
//
//
//|        CallOneThread |   100000 |     2,299.177 us |     43.3314 us |     40.5322 us |     2,278.480 us |
//
//|         ParallelFor1 |   100000 |     2,382.346 us |     14.4527 us |     13.5191 us |     2,376.574 us |
//|           TaskBased1 |   100000 |     2,379.525 us |      2.9294 us |      2.5969 us |     2,379.826 us |
//
//|         ParallelFor2 |   100000 |     1,260.044 us |     16.2613 us |     15.2108 us |     1,257.726 us |
//|           TaskBased2 |   100000 |     1,304.253 us |     25.3692 us |     39.4969 us |     1,288.518 us |
//
//|         ParallelFor4 |   100000 |       674.734 us |      8.2369 us |      7.3018 us |       673.525 us |
//|           TaskBased4 |   100000 |       904.866 us |     10.5658 us |      9.3663 us |       904.223 us |
//
//|         ParallelFor6 |   100000 |       686.318 us |      5.8302 us |      5.4535 us |       688.436 us |
//|           TaskBased6 |   100000 |       691.203 us |      6.0545 us |      5.0558 us |       691.012 us |
//
//|         ParallelFor8 |   100000 |       530.633 us |      5.2931 us |      4.6922 us |       529.096 us |
//|           TaskBased8 |   100000 |       597.014 us |      2.2443 us |      2.0994 us |       595.945 us |
//
//| ParallelForCoreCount |   100000 |       429.565 us |      1.0862 us |      1.0160 us |       429.533 us |
//|   TaskBasedCoreCount |   100000 |       380.238 us |      3.9910 us |      3.7332 us |       380.339 us |
//
//
//|        CallOneThread |   500000 |    11,469.126 us |     42.7052 us |     35.6608 us |    11,463.970 us |
//
//|         ParallelFor1 |   500000 |    11,561.992 us |     39.6977 us |     35.1910 us |    11,547.953 us |
//|           TaskBased1 |   500000 |    11,617.365 us |     36.7953 us |     30.7258 us |    11,606.156 us |
//
//|         ParallelFor2 |   500000 |     7,367.180 us |      7.8051 us |      6.9190 us |     7,367.389 us |
//|           TaskBased2 |   500000 |     6,033.361 us |     94.8258 us |     88.7001 us |     5,985.369 us |
//
//|         ParallelFor4 |   500000 |     4,689.393 us |    106.6915 us |    300.9254 us |     4,785.842 us |
//|           TaskBased4 |   500000 |     4,129.548 us |     82.4961 us |    212.9487 us |     4,131.341 us |
//
//|         ParallelFor6 |   500000 |     3,567.294 us |     38.9846 us |     36.4662 us |     3,567.434 us |
//|           TaskBased6 |   500000 |     3,444.276 us |     14.6833 us |     13.7347 us |     3,436.127 us |
//
//|         ParallelFor8 |   500000 |     3,242.603 us |     25.2307 us |     23.6008 us |     3,246.214 us |
//|           TaskBased8 |   500000 |     3,117.051 us |      9.9261 us |      8.7992 us |     3,115.579 us |
//
//| ParallelForCoreCount |   500000 |     2,953.354 us |      7.5246 us |      6.2834 us |     2,952.409 us |
//|   TaskBasedCoreCount |   500000 |     2,951.588 us |      7.4457 us |      6.2175 us |     2,950.184 us |
//
//
//|        CallOneThread |  5000000 |   108,906.037 us |    548.0373 us |    457.6360 us |   108,840.620 us |
//
//|         ParallelFor1 |  5000000 |   109,086.243 us |    566.1959 us |    472.7992 us |   108,868.720 us |
//|           TaskBased1 |  5000000 |   108,891.326 us |    333.5246 us |    278.5081 us |   108,783.360 us |
//
//|         ParallelFor2 |  5000000 |    68,380.613 us |    127.0478 us |    118.8406 us |    68,362.733 us |
//|           TaskBased2 |  5000000 |    56,093.547 us |    450.9807 us |    421.8477 us |    56,052.167 us |
//
//|         ParallelFor4 |  5000000 |    37,198.997 us |    465.6852 us |    435.6023 us |    36,967.543 us |
//|           TaskBased4 |  5000000 |    37,565.921 us |    737.5408 us |  1,310.9781 us |    37,495.481 us |
//
//|         ParallelFor6 |  5000000 |    35,123.051 us |    697.2118 us |  1,145.5390 us |    34,757.773 us |
//|           TaskBased6 |  5000000 |    36,917.037 us |    263.7797 us |    246.7397 us |    36,891.050 us |
//
//|         ParallelFor8 |  5000000 |    35,087.684 us |    456.6135 us |    427.1166 us |    35,200.107 us |
//|           TaskBased8 |  5000000 |    34,278.382 us |    280.1776 us |    262.0783 us |    34,298.833 us |
//
//| ParallelForCoreCount |  5000000 |    33,554.689 us |     66.8662 us |     59.2752 us |    33,542.210 us |
//|   TaskBasedCoreCount |  5000000 |    33,506.573 us |     73.4559 us |     65.1167 us |    33,510.680 us |
//
//
//|        CallOneThread | 50000000 | 1,096,138.080 us | 14,499.1372 us | 13,562.5019 us | 1,091,201.500 us |
//
//|         ParallelFor1 | 50000000 | 1,087,767.364 us |  5,432.9328 us |  4,816.1534 us | 1,086,863.200 us |
//|           TaskBased1 | 50000000 | 1,084,586.208 us |  2,877.0245 us |  2,402.4457 us | 1,084,224.100 us |
//
//|         ParallelFor2 | 50000000 |   554,862.050 us |  1,111.9193 us |    868.1135 us |   554,733.850 us |
//|           TaskBased2 | 50000000 |   557,646.233 us |  3,622.8347 us |  3,388.8018 us |   556,587.300 us |
//
//|         ParallelFor4 | 50000000 |   366,375.500 us |  5,238.6597 us |  4,900.2455 us |   364,862.600 us |
//|           TaskBased4 | 50000000 |   347,147.477 us |  6,864.8810 us |  9,396.7266 us |   348,379.900 us |
//
//|         ParallelFor6 | 50000000 |   361,699.368 us |  6,383.6342 us |  7,839.6761 us |   361,208.125 us |
//|           TaskBased6 | 50000000 |   338,370.923 us |  5,387.3563 us |  4,498.6865 us |   339,551.600 us |
//
//|         ParallelFor8 | 50000000 |   338,134.773 us |  5,192.7848 us |  4,857.3341 us |   338,524.300 us |
//|           TaskBased8 | 50000000 |   344,908.407 us |  5,368.8064 us |  5,021.9848 us |   342,702.600 us |
//
//| ParallelForCoreCount | 50000000 |   335,402.929 us |    714.7325 us |    633.5917 us |   335,197.850 us |
//|   TaskBasedCoreCount | 50000000 |   335,764.193 us |    689.4130 us |    644.8773 us |   335,802.800 us |

#endregion

#region i5

//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22621
//13th Gen Intel Core i5-13600K, 1 CPU, 20 logical and 14 physical cores
//.NET SDK=7.0.302
//  [Host]     : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT
//  DefaultJob : .NET 6.0.16 (6.0.1623.17311), X64 RyuJIT
//
//
//|               Method |    Count |             Mean |          Error |          StdDev |           Median |
//|--------------------- |--------- |-----------------:|---------------:|----------------:|-----------------:|
//|        CallOneThread |      100 |         3.140 us |      0.1330 us |       0.3921 us |         3.224 us |
//|         ParallelFor1 |      100 |         4.032 us |      0.1857 us |       0.5477 us |         4.167 us |
//|           TaskBased1 |      100 |         6.445 us |      0.1197 us |       0.2627 us |         6.424 us |
//|         ParallelFor2 |      100 |         4.202 us |      0.0837 us |       0.1227 us |         4.225 us |
//|           TaskBased2 |      100 |         4.514 us |      0.1497 us |       0.4048 us |         4.529 us |
//|         ParallelFor4 |      100 |         3.978 us |      0.0308 us |       0.0273 us |         3.972 us |
//|           TaskBased4 |      100 |        14.268 us |      0.7039 us |       2.0755 us |        14.875 us |
//|         ParallelFor6 |      100 |         4.271 us |      0.0836 us |       0.1087 us |         4.252 us |
//|           TaskBased6 |      100 |        10.232 us |      0.9631 us |       2.8398 us |        11.214 us |
//|         ParallelFor8 |      100 |         4.355 us |      0.0293 us |       0.0260 us |         4.351 us |
//|           TaskBased8 |      100 |        11.343 us |      0.6331 us |       1.8668 us |        11.624 us |
//| ParallelForCoreCount |      100 |         7.177 us |      0.1422 us |       0.2297 us |         7.170 us |
//|   TaskBasedCoreCount |      100 |         9.647 us |      0.5521 us |       1.6279 us |         9.183 us |
//
//
//|        CallOneThread |     1000 |        32.384 us |      0.0072 us |       0.0067 us |        32.384 us |
//|         ParallelFor1 |     1000 |        31.419 us |      0.3430 us |       0.3208 us |        31.546 us |
//|           TaskBased1 |     1000 |        32.852 us |      0.7966 us |       2.3489 us |        33.181 us |
//|         ParallelFor2 |     1000 |        19.632 us |      0.3713 us |       0.3473 us |        19.604 us |
//|           TaskBased2 |     1000 |        22.912 us |      0.4561 us |       1.2253 us |        23.051 us |
//|         ParallelFor4 |     1000 |        18.175 us |      0.3628 us |       0.9429 us |        18.337 us |
//|           TaskBased4 |     1000 |        16.448 us |      0.3287 us |       0.8660 us |        16.804 us |
//|         ParallelFor6 |     1000 |        13.383 us |      0.2868 us |       0.8456 us |        13.555 us |
//|           TaskBased6 |     1000 |        19.265 us |      0.3778 us |       0.4351 us |        19.344 us |
//|         ParallelFor8 |     1000 |        10.688 us |      0.2117 us |       0.3036 us |        10.565 us |
//|           TaskBased8 |     1000 |        22.692 us |      0.5493 us |       1.5760 us |        22.892 us |
//| ParallelForCoreCount |     1000 |        10.981 us |      0.2136 us |       0.1893 us |        11.025 us |
//|   TaskBasedCoreCount |     1000 |        23.796 us |      0.4731 us |       0.6151 us |        23.886 us |
//
//
//|        CallOneThread |     5000 |       126.186 us |      5.8655 us |      16.1553 us |       131.282 us |
//|         ParallelFor1 |     5000 |       153.630 us |      7.6717 us |      22.6203 us |       158.087 us |
//|           TaskBased1 |     5000 |       149.014 us |      6.0427 us |      17.8171 us |       151.850 us |
//|         ParallelFor2 |     5000 |        90.227 us |      2.3621 us |       6.9648 us |        88.588 us |
//|           TaskBased2 |     5000 |       121.452 us |      7.5081 us |      22.1379 us |       129.741 us |
//|         ParallelFor4 |     5000 |        55.030 us |      1.0880 us |       2.6278 us |        55.531 us |
//|           TaskBased4 |     5000 |        60.570 us |      1.2083 us |       2.4408 us |        60.392 us |
//|         ParallelFor6 |     5000 |        39.343 us |      0.7794 us |       1.6439 us |        38.996 us |
//|           TaskBased6 |     5000 |        38.770 us |      0.7726 us |       1.9944 us |        38.429 us |
//|         ParallelFor8 |     5000 |        44.075 us |      0.8684 us |       1.3261 us |        44.073 us |
//|           TaskBased8 |     5000 |        38.855 us |      0.7570 us |       0.7081 us |        38.886 us |
//| ParallelForCoreCount |     5000 |        28.026 us |      0.2754 us |       0.2576 us |        28.024 us |
//|   TaskBasedCoreCount |     5000 |        38.537 us |      0.8344 us |       2.4074 us |        38.859 us |
//
//
//|        CallOneThread |    10000 |       252.538 us |     13.0242 us |      34.5382 us |       262.183 us |
//|         ParallelFor1 |    10000 |       248.300 us |     18.7619 us |      55.3200 us |       274.912 us |
//|           TaskBased1 |    10000 |       294.172 us |     21.8368 us |      64.3864 us |       316.709 us |
//|         ParallelFor2 |    10000 |       179.491 us |      5.9692 us |      17.5068 us |       182.258 us |
//|           TaskBased2 |    10000 |       227.304 us |      4.3207 us |       4.0416 us |       227.466 us |
//|         ParallelFor4 |    10000 |       135.098 us |      6.9139 us |      20.3857 us |       134.010 us |
//|           TaskBased4 |    10000 |       105.623 us |      2.4569 us |       7.2442 us |       106.618 us |
//|         ParallelFor6 |    10000 |        79.215 us |      1.1133 us |       0.9296 us |        79.036 us |
//|           TaskBased6 |    10000 |        75.340 us |      2.2879 us |       6.2245 us |        76.994 us |
//|         ParallelFor8 |    10000 |        61.726 us |      1.2147 us |       1.5362 us |        61.114 us |
//|           TaskBased8 |    10000 |        56.585 us |      0.7317 us |       0.6110 us |        56.366 us |
//| ParallelForCoreCount |    10000 |        55.882 us |      3.5265 us |      10.2868 us |        60.159 us |
//|   TaskBasedCoreCount |    10000 |        58.131 us |      3.2699 us |       9.2227 us |        60.897 us |
//
//
//|        CallOneThread |    50000 |     1,474.941 us |    123.2288 us |     363.3428 us |     1,644.423 us |
//|         ParallelFor1 |    50000 |     1,294.329 us |     62.4111 us |     184.0207 us |     1,334.162 us |
//|           TaskBased1 |    50000 |     1,531.687 us |     55.7730 us |     164.4481 us |     1,548.105 us |
//|         ParallelFor2 |    50000 |     1,090.139 us |     21.6465 us |      46.1304 us |     1,098.558 us |
//|           TaskBased2 |    50000 |       834.330 us |      1.1268 us |       1.0540 us |       834.485 us |
//|         ParallelFor4 |    50000 |       622.165 us |     46.1251 us |     136.0010 us |       672.078 us |
//|           TaskBased4 |    50000 |       401.359 us |     35.4699 us |      90.9228 us |       438.257 us |
//|         ParallelFor6 |    50000 |       539.859 us |     87.5893 us |     258.2590 us |       626.138 us |
//|           TaskBased6 |    50000 |       132.349 us |      2.5558 us |       2.5102 us |       131.628 us |
//|         ParallelFor8 |    50000 |       220.623 us |      4.3394 us |       6.2234 us |       221.638 us |
//|           TaskBased8 |    50000 |       220.894 us |      0.2575 us |       0.2282 us |       220.922 us |
//| ParallelForCoreCount |    50000 |       312.664 us |     21.5887 us |      63.6547 us |       314.473 us |
//|   TaskBasedCoreCount |    50000 |       257.133 us |      4.8744 us |       5.6134 us |       257.464 us |
//
//
//|        CallOneThread |   100000 |     3,186.183 us |    102.3056 us |     300.0446 us |     3,237.693 us |
//|         ParallelFor1 |   100000 |     3,030.503 us |    235.7251 us |     641.3074 us |     3,278.239 us |
//|           TaskBased1 |   100000 |     3,084.635 us |    126.2678 us |     372.3034 us |     3,172.427 us |
//|         ParallelFor2 |   100000 |     1,956.918 us |     75.0941 us |     221.4166 us |     2,021.511 us |
//|           TaskBased2 |   100000 |     1,663.525 us |     40.2707 us |     118.1070 us |     1,681.023 us |
//|         ParallelFor4 |   100000 |     1,174.790 us |    118.6266 us |     349.7732 us |     1,322.166 us |
//|           TaskBased4 |   100000 |       873.048 us |     27.2989 us |      80.4914 us |       889.411 us |
//|         ParallelFor6 |   100000 |     1,388.887 us |    103.9921 us |     306.6230 us |     1,541.085 us |
//|           TaskBased6 |   100000 |       733.608 us |     32.9315 us |      96.5824 us |       732.577 us |
//|         ParallelFor8 |   100000 |       733.171 us |     44.7763 us |     132.0241 us |       718.733 us |
//|           TaskBased8 |   100000 |       480.790 us |     15.8452 us |      41.7426 us |       471.284 us |
//| ParallelForCoreCount |   100000 |       634.378 us |     20.9062 us |      61.6425 us |       640.636 us |
//|   TaskBasedCoreCount |   100000 |       453.680 us |      4.3139 us |       3.8242 us |       453.352 us |
//
//
//|        CallOneThread |   500000 |    14,679.724 us |    666.6304 us |   1,965.5747 us |    15,256.114 us |
//|         ParallelFor1 |   500000 |    16,628.625 us |    331.7741 us |     735.1884 us |    16,723.662 us |
//|           TaskBased1 |   500000 |    15,614.818 us |    355.8125 us |   1,049.1212 us |    15,580.340 us |
//|         ParallelFor2 |   500000 |     5,472.968 us |    783.9370 us |   2,311.4558 us |     4,117.196 us |
//|           TaskBased2 |   500000 |     7,421.009 us |    563.7533 us |   1,662.2394 us |     8,278.766 us |
//|         ParallelFor4 |   500000 |     5,743.488 us |    385.9203 us |   1,137.8948 us |     6,062.130 us |
//|           TaskBased4 |   500000 |     5,311.930 us |    236.7861 us |     690.7161 us |     5,471.574 us |
//|         ParallelFor6 |   500000 |     6,395.220 us |    320.0017 us |     943.5322 us |     6,719.727 us |
//|           TaskBased6 |   500000 |     4,066.504 us |      9.1215 us |       7.1215 us |     4,067.972 us |
//|         ParallelFor8 |   500000 |     3,703.745 us |    134.0138 us |     395.1428 us |     3,684.111 us |
//|           TaskBased8 |   500000 |     3,128.739 us |    158.4951 us |     433.8777 us |     3,216.009 us |
//| ParallelForCoreCount |   500000 |     1,167.511 us |     14.2940 us |      12.6713 us |     1,163.598 us |
//|   TaskBasedCoreCount |   500000 |     3,010.146 us |     98.9866 us |     291.8641 us |     3,055.637 us |
//
//
//|        CallOneThread |  5000000 |   161,415.650 us |  7,063.0047 us |  20,825.4287 us |   166,318.136 us |
//|         ParallelFor1 |  5000000 |   130,346.982 us |  4,739.1311 us |  12,812.5165 us |   133,053.743 us |
//|           TaskBased1 |  5000000 |   157,564.923 us |  3,143.0879 us |   7,346.8666 us |   157,866.675 us |
//|         ParallelFor2 |  5000000 |    84,752.497 us |  1,030.4524 us |     963.8859 us |    84,429.131 us |
//|           TaskBased2 |  5000000 |    82,913.544 us |  1,611.3114 us |   2,692.1394 us |    83,361.169 us |
//|         ParallelFor4 |  5000000 |    45,325.187 us |    636.0452 us |     594.9571 us |    45,484.700 us |
//|           TaskBased4 |  5000000 |    42,589.818 us |    403.2487 us |     377.1991 us |    42,588.322 us |
//|         ParallelFor6 |  5000000 |    37,359.353 us |  1,750.0380 us |   5,160.0266 us |    38,668.373 us |
//|           TaskBased6 |  5000000 |    32,910.449 us |    181.9152 us |     170.1636 us |    32,907.005 us |
//|         ParallelFor8 |  5000000 |    30,592.931 us |    608.3154 us |   1,654.9668 us |    30,744.758 us |
//|           TaskBased8 |  5000000 |    27,676.734 us |    552.8607 us |     953.6575 us |    27,856.097 us |
//| ParallelForCoreCount |  5000000 |    29,200.809 us |    831.1170 us |   2,450.5673 us |    29,730.778 us |
//|   TaskBasedCoreCount |  5000000 |    28,831.555 us |    558.9982 us |     621.3250 us |    28,970.247 us |
//
//
//|        CallOneThread | 50000000 | 1,330,203.893 us |    546.6177 us |     484.5624 us | 1,330,302.800 us |
//|         ParallelFor1 | 50000000 | 1,390,217.643 us | 37,391.8948 us | 110,250.8454 us | 1,335,229.800 us |
//|           TaskBased1 | 50000000 | 1,325,727.135 us | 26,243.4324 us |  69,135.6980 us | 1,334,221.100 us |
//|         ParallelFor2 | 50000000 |   841,301.445 us | 54,177.8275 us | 159,744.5468 us |   960,439.400 us |
//|           TaskBased2 | 50000000 |   818,185.993 us | 16,010.3809 us |  28,870.0192 us |   831,440.600 us |
//|         ParallelFor4 | 50000000 |   442,160.313 us |  8,270.8339 us |   7,736.5431 us |   441,432.300 us |
//|           TaskBased4 | 50000000 |   557,835.433 us | 11,430.9966 us |  33,704.5514 us |   566,045.650 us |
//|         ParallelFor6 | 50000000 |   348,936.606 us |  8,828.8168 us |  26,031.9656 us |   348,380.300 us |
//|           TaskBased6 | 50000000 |   332,514.554 us |  6,612.5196 us |  17,535.4640 us |   331,419.000 us |
//|         ParallelFor8 | 50000000 |   272,045.163 us |  5,818.0689 us |  16,971.5801 us |   272,456.150 us |
//|           TaskBased8 | 50000000 |   294,058.810 us |  1,572.8697 us |   1,471.2633 us |   293,878.775 us |
//| ParallelForCoreCount | 50000000 |   281,326.305 us |  5,556.1369 us |   5,197.2139 us |   281,370.225 us |
//|   TaskBasedCoreCount | 50000000 |   276,933.481 us |  5,364.3225 us |   5,268.4820 us |   275,448.200 us |

#endregion

abstract class RunnerParallelFor<TElement, TState>
{
    private readonly Action<int> body;
    protected TElement[]? Data;
    protected TState State;

    protected RunnerParallelFor()
    {
        body = Handle;
    }

    public void Run(TElement[] array, TState state)
    {
        Data = array;
        State = state;

        int batchCount = GetThreadsCount();
        PrepareState(array.Length / batchCount);
        Parallel.For(0, batchCount, body);

        Data = null;
    }

    protected abstract int GetThreadsCount();

    protected abstract void PrepareState(int batchSize);

    protected abstract void Handle(int batchId);
}

class ForRunner : RunnerParallelFor<Matrix4F, Matrix4F[]>
{
    private readonly int threads;
    private int countInBatch;

    public ForRunner(int threads)
    {
        this.threads = threads;
    }

    protected override int GetThreadsCount() => threads;

    protected override void PrepareState(int batchSize)
    {
        countInBatch = batchSize;
    }

    protected override void Handle(int batchId)
    {
        int start = batchId * countInBatch;
        int stop = (batchId + 1) * countInBatch;
        TypeOfParallelizationTest.Process(Data, State, start, stop);
    }
}

abstract class RunnerTasksBase<TElement, TState>
{
    private Task[]? tasks;
    protected TElement[]? Data;
    protected TState? State;

    public void Run(TElement[] array, TState? state)
    {
        Data = array;
        State = state;

        int batchCount = GetThreadsCount();
        PrepareState(array.Length / batchCount);
        if (tasks == null || tasks.Length != batchCount)
            tasks = new Task[batchCount];
        for (int i = 0; i < batchCount; i++)
        {
            tasks[i] = Task.Factory.StartNew(Handle, i);
        }
        Task.WaitAll(tasks);
    }

    protected abstract int GetThreadsCount();

    protected abstract void PrepareState(int batchSize);

    protected abstract void Handle(object? info);
}

class TaskRunner : RunnerTasksBase<Matrix4F, Matrix4F[]>
{
    private readonly int threadsCount;
    private int countInBatch;

    public TaskRunner(int threadsCount)
    {
        this.threadsCount = threadsCount;
    }

    protected override int GetThreadsCount() => threadsCount;

    protected override void PrepareState(int batchSize)
    {
        countInBatch = batchSize;
    }

    protected override void Handle(object? info)
    {
        int batchIdd = (int)info!;
        int start = batchIdd * countInBatch;
        int stop = (batchIdd + 1) * countInBatch;
        TypeOfParallelizationTest.Process(Data!, State, start, stop);
    }
}

public class TypeOfParallelizationTest
{
    public static void Process(Matrix4F[] input, Matrix4F[] output, int startIndex, int stopExcl)
    {
        for (int i = startIndex; i < stopExcl; i++)
        {
            Matrix4F.Invert(input[i], out output[i]);
        }
    }

    private readonly ForRunner for1 = new ForRunner(1);
    private readonly ForRunner for2 = new ForRunner(2);
    private readonly ForRunner for4 = new ForRunner(4);
    private readonly ForRunner for6 = new ForRunner(6);
    private readonly ForRunner for8 = new ForRunner(8);
    private readonly ForRunner forX = new ForRunner(Environment.ProcessorCount);
    private readonly TaskRunner tasks1 = new TaskRunner(1);
    private readonly TaskRunner tasks2 = new TaskRunner(2);
    private readonly TaskRunner tasks4 = new TaskRunner(4);
    private readonly TaskRunner tasks6 = new TaskRunner(6);
    private readonly TaskRunner tasks8 = new TaskRunner(8);
    private readonly TaskRunner tasksX = new TaskRunner(Environment.ProcessorCount);

    private Matrix4F[] matrices = null!;
    private Matrix4F[] result = null!;

    [Params(100, 1000, 5000, 10_000, 50_000, 100_000, 500_000, 5_000_000, 50_000_000)]
    public int Count { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        matrices = ParallelExecutionTest.GetRandomMatrices(Count);
        result = new Matrix4F[matrices.Length];
    }

    [Benchmark] public void CallOneThread() => Process(matrices, result, 0, matrices.Length);
    
    [Benchmark] public void ParallelFor1() => for1.Run(matrices, result);
    [Benchmark] public void TaskBased1() => tasks1.Run(matrices, result);

    [Benchmark] public void ParallelFor2() => for2.Run(matrices, result);
    [Benchmark] public void TaskBased2() => tasks2.Run(matrices, result);

    [Benchmark] public void ParallelFor4() => for4.Run(matrices, result);
    [Benchmark] public void TaskBased4() => tasks4.Run(matrices, result);

    [Benchmark] public void ParallelFor6() => for6.Run(matrices, result);
    [Benchmark] public void TaskBased6() => tasks6.Run(matrices, result);

    [Benchmark] public void ParallelFor8() => for8.Run(matrices, result);
    [Benchmark] public void TaskBased8() => tasks8.Run(matrices, result);

    [Benchmark] public void ParallelForCoreCount() => forX.Run(matrices, result);
    [Benchmark] public void TaskBasedCoreCount() => tasksX.Run(matrices, result);
}