# OpenTelemetry

The project contains an observability stack which is started when running `docker compose up`.
It consists of the following components:
- `otel-collector` - [OpenTelemetry Collector](https://opentelemetry.io/docs/collector/)
   which collects the telemetry send by the applications.
- `jaeger` - [Jaeger](https://www.jaegertracing.io/) as traces backend
- `prometheus` - [Prometheus](https://prometheus.io/) as metrics backend
- `loki` - [Grafana Loki](https://grafana.com/oss/loki/) as logs backend
- `grafana` - [Grafana](https://grafana.com/oss/grafana/) as telemetry UI

The `otel-collector` receives traces, logs and metrics from the instrumented applications.
It exports the traces to Jaeger, the metrics to Prometheus and the logs to Loki.
Grafana is used to visualize the data from all three backends.

Grafana can be accessed at `http://localhost:3000`.
In the `explore` section you can query the data from the different backends.
