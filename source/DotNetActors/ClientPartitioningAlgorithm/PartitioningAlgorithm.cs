using AkkaSb.Net;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetActors.ClientPartitioningAlgorithm
{
    /// <summary>
    /// Helper class that implemented the optimal partitioning algortihm.
    /// It is useful when the client knows the number of actors and nodes that will be used.
    /// </summary>
    internal class PartitioningAlgorithm
    {
        List<Placement<int>> map = new List<Placement<int>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numElements">The number of actor that ewill be used.</param>
        /// <param name="numOfElementsPerPartition">Actors are grouped in partitions (slots) and placed at all nodes.</param>
        /// <param name="numOfPartitions"></param>
        /// <param name="nodes"></param>
        public PartitioningAlgorithm(int numElements, int numOfElementsPerPartition, int numOfPartitions, List<string> nodes)
        {
            this.map = CreatePartitionMap(numElements, numOfElementsPerPartition, nodes, numOfPartitions);
        }


        /// <summary>
        /// Creates the list of actors to be used and their map. If the sustem knows the number of node and the number of actors to be used,
        /// this method can be used to cretae a partition map. Once the map is created the client 
        /// </summary>
        /// <param name="numElements"></param>
        /// <param name="numOfElementsPerPartition"></param>
        /// <param name="numOfPartitions">Do not use this parameter if the number of partitions should equel number of nodes. Recommended value. Using some other values is an experminetal approach.</param>
        /// <param name="nodes"></param>
        /// <returns></returns>
        private List<Placement<int>> CreatePartitionMap(int numElements, int numOfPartitions, List<string> nodes, int numOfElementsPerPartition = -1)
        {
            List<Placement<int>>  map = new List<Placement<int>>();

            if (numOfPartitions == -1)
                numOfPartitions = (numOfElementsPerPartition == -1 && nodes != null && nodes.Count > 0) ? nodes.Count : (int)(1 + ((double)numElements / (double)numOfElementsPerPartition));

            if (numOfElementsPerPartition == -1)
            {
                var ratio = (float)numElements / (float)numOfPartitions;

                if (ratio > 0)
                    numOfElementsPerPartition = (int)(1.0 + ratio);
                else
                    numOfElementsPerPartition = (int)ratio;
            }

            int destNodeIndx = 0;
            string destinationNode = null;

            for (int partIndx = 0; partIndx < numOfPartitions; partIndx++)
            {
                var min = numOfElementsPerPartition * partIndx;
                var maxPartEl = numOfElementsPerPartition * (partIndx + 1) - 1;
                var max = maxPartEl < numElements ? maxPartEl : numElements - 1;

                if (min >= numElements)
                    break;

                if (nodes != null && nodes.Count > 0)
                {
                    destNodeIndx %= nodes.Count;
                    destinationNode = nodes[destNodeIndx++];
                }

                map.Add(new Placement<int>() { NodeIndx = -1, NodePath = destinationNode, PartitionIndx = partIndx, MinKey = min, MaxKey = max, ActorRef = null });
            }

            return map;
        }

        /// <summary>
        /// This method is used to get the path of the actor.
        /// </summary>
        /// <param name="key">The actor identifier for which the path has to be mapped.</param>
        /// <returns></returns>
        public ActorReference GetPartitionActorFromKey(int key)
        {
            return this.map.Where(p => p.MinKey <= key && p.MaxKey >= key).First().ActorRef as ActorReference;
        }

    }
}
