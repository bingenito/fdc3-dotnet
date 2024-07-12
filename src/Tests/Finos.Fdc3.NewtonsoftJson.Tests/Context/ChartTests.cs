﻿/*
 * Morgan Stanley makes this available to you under the Apache License,
 * Version 2.0 (the "License"). You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0.
 *
 * See the NOTICE file distributed with this work for additional information
 * regarding copyright ownership. Unless required by applicable law or agreed
 * to in writing, software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express
 * or implied. See the License for the specific language governing permissions
 * and limitations under the License.
 */

using Finos.Fdc3.Context;

namespace Finos.Fdc3.NewtonsoftJson.Tests.Context;

public class ChartTests : ContextSchemaTest
{
    public ChartTests()
        : base("https://fdc3.finos.org/schemas/2.0/chart.schema.json")
    {
    }

    [Fact]
    public async Task Chart_SerializedJsonMatchesSchema()
    {
        Instrument instrument = new Instrument(new InstrumentID { Ticker = "TICKER" });
        TimeRange timeRange = new TimeRange(DateTime.Now.ToString("o"), DateTime.Now.ToString("o"));
        var otherConfig = new { A = "Foo", B = "Bar" };
        Chart chart = new Chart(new Instrument[] { instrument }, timeRange, otherConfig, ChartStyle.Line, null, "chart");
        await this.ValidateSchema(chart);
    }
}