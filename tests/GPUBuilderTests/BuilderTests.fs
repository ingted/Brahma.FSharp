﻿open OpenCL.Net
open Brahma.OpenCL
open Brahma.FSharp.OpenCL.Core
open Microsoft.FSharp.Quotations
open Brahma.FSharp.OpenCL.Extensions 
open ArrayGPU
open BrahmaBuilder
open NUnit.Framework

[<TestFixture>]
type ``Brahma Workflow Builder tests`` () = 
    
    [<Test>]
    member this.``Workflow composition test`` () = 
        
    //defining context
        let platformName = "NVIDIA*"
        let deviceType = DeviceType.Default        
        let provider1 =
                try  ComputeProvider.Create(platformName, deviceType)
                with 
                | ex -> failwith ex.Message
        let mutable commandQueue1 = new CommandQueue(provider1, provider1.Devices |> Seq.head) 
        let length1 = 5
    
        let getLocalWorkSize1 (length) = 
            let lws, ex = OpenCL.Net.Cl.GetDeviceInfo(provider1.Devices |> Seq.head, OpenCL.Net.DeviceInfo.MaxWorkGroupSize)
            let maxWorkSize = int <| lws.CastTo<uint64>()
            if length <= maxWorkSize then length
            else 
                let mutable l = maxWorkSize
                while (length % l <> 0) do 
                    l <- l - 1
                l
    
        let localworksize1 = getLocalWorkSize1 length1
    

        let actcontext = provider1, commandQueue1, length1, localworksize1
       
        let testarr = [|5; 7; 8; 22; 16|] 
        
        let gpu = new BrahmaBuilder.GPUBuilder(actcontext)
    
        let outerFunc inArr =
                          gpu 
                            {                             
                                let! e = ArrayGPU.Reverse inArr
                                let! f = ArrayGPU.Map <@ fun a -> a + 1 @> e
                                yield f
                            } 

        let func1 a = 
                  gpu 
                    {                        
                     let! c = ArrayGPU.Reverse a
                     let! d = outerFunc a
                     let! g = ArrayGPU.Map2 <@ fun a b -> a + b @> c d
                     return g
                    } 
        let computation1 = func1 testarr
        //Now we have to unwrap the value which we get from the computation
        //To do that we use the run function from the Reader module with the same context that we use in the computation
        let test1 = BrahmaBuilder.run actcontext computation1
        //We can see, that it works just as expected  
        Assert.AreSame  (test1, [|33; 45; 17; 15; 11;|])
    [<Test>]
    member this.``Workflow simple test`` () = 
        let platformName = "NVIDIA*"
        let deviceType = DeviceType.Default    
        let provider2 =
                try  ComputeProvider.Create(platformName, deviceType)
                with 
                | ex -> failwith ex.Message
        let mutable commandQueue2 = new CommandQueue(provider2, provider2.Devices |> Seq.head) 
        let length2 = 6
        let getLocalWorkSize2 (length) = 
            let lws, ex = OpenCL.Net.Cl.GetDeviceInfo(provider2.Devices |> Seq.head, OpenCL.Net.DeviceInfo.MaxWorkGroupSize)
            let maxWorkSize = int <| lws.CastTo<uint64>()
            if length <= maxWorkSize then length
            else 
                let mutable l = maxWorkSize
                while (length % l <> 0) do 
                    l <- l - 1
                l
       
        let localworksize2 = getLocalWorkSize2 length2
    

        let actcontext2 = provider2, commandQueue2, length2, localworksize2
        let gpu2 = new BrahmaBuilder.GPUBuilder(actcontext2)
        
        let computation2 =
                      gpu2 
                            { 
                            let a = [|5; 7; 8; 22; 16; 0|]             
                            let! c = ArrayGPU.Reverse a
                            let! d = ArrayGPU.Map <@ fun a -> a + 1 @> c
                            return d
                            } 
        let test2 = BrahmaBuilder.run actcontext2 computation2
        Assert.AreSame  (test2, [|1; 17; 23; 9; 8; 6;|])
