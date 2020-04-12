#include <iostream>

#include <chrono> 
using namespace std::chrono;


class Stopwatch
{
    steady_clock::time_point startTime;

public:
	void Restart()
	{
        startTime = high_resolution_clock::now();
	}

	long long StopAndGetMilliseconds() const
	{
        const steady_clock::time_point stop = high_resolution_clock::now();
        return duration_cast<microseconds>(stop - startTime).count();
	}
};

void Do()
{
	
}

class Measurement
{
    Stopwatch timer;

public:

    double Run(int runCount)
    {
        timer.Restart();
        for (int i = 0; i < runCount; ++i)
        {
            
        }
        const long long time = timer.StopAndGetMilliseconds();
        return static_cast<double>(time) / runCount;
    }
};

void main()
{
    const int initialTimeMeasureCount = 1000;
    const int targetMeasurementSeconds = 10;

    Measurement measurement;
    const double timeWarmUpRun = measurement.Run(initialTimeMeasureCount);

    const int targetRunCount = static_cast<int>(targetMeasurementSeconds / timeWarmUpRun);
    const double time = measurement.Run(targetRunCount);

    std::cout << time;
}
